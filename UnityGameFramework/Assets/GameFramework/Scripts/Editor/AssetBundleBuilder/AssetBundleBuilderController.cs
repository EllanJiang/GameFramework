//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using ICSharpCode.SharpZipLib.GZip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleBuilderController : Utility.Zip.IZipHelper
    {
        private const string ConfigurationName = "GameFramework/Configs/AssetBundleBuilder.xml";
        private const string VersionListFileName = "version";
        private const string ResourceListFileName = "list";
        private const string RecordName = "GameResourceVersion";
        private static readonly char[] PackageListHeader = new char[] { 'E', 'L', 'P' };
        private static readonly char[] VersionListHeader = new char[] { 'E', 'L', 'V' };
        private static readonly char[] ReadOnlyListHeader = new char[] { 'E', 'L', 'R' };
        private const byte PackageListVersion = 0;
        private const byte VersionListVersion = 0;
        private const byte ReadOnlyListVersion = 0;
        private const int QuickEncryptLength = 220;
        private readonly int AssetsSubstringLength = "Assets/".Length;

        private readonly AssetBundleCollection m_AssetBundleCollection;
        private readonly AssetBundleAnalyzerController m_AssetBundleAnalyzerController;
        private readonly SortedDictionary<string, AssetBundleData> m_AssetBundleDatas;
        private readonly Dictionary<BuildTarget, VersionListData> m_VersionListDatas;
        private readonly BuildReport m_BuildReport;

        public AssetBundleBuilderController()
        {
            m_AssetBundleCollection = new AssetBundleCollection();

            m_AssetBundleCollection.OnLoadingAssetBundle += delegate (int index, int count)
            {
                if (OnLoadingAssetBundle != null)
                {
                    OnLoadingAssetBundle(index, count);
                }
            };

            m_AssetBundleCollection.OnLoadingAsset += delegate (int index, int count)
            {
                if (OnLoadingAsset != null)
                {
                    OnLoadingAsset(index, count);
                }
            };

            m_AssetBundleCollection.OnLoadCompleted += delegate ()
            {
                if (OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }
            };

            m_AssetBundleAnalyzerController = new AssetBundleAnalyzerController(m_AssetBundleCollection);

            m_AssetBundleAnalyzerController.OnAnalyzingAsset += delegate (int index, int count)
            {
                if (OnAnalyzingAsset != null)
                {
                    OnAnalyzingAsset(index, count);
                }
            };

            m_AssetBundleAnalyzerController.OnAnalyzeCompleted += delegate ()
            {
                if (OnAnalyzeCompleted != null)
                {
                    OnAnalyzeCompleted();
                }
            };

            m_AssetBundleDatas = new SortedDictionary<string, AssetBundleData>();
            m_VersionListDatas = new Dictionary<BuildTarget, VersionListData>();
            m_BuildReport = new BuildReport();

            WindowsSelected = MacOSXSelected = IOSSelected = AndroidSelected = WindowsStoreSelected = true;
            ZipSelected = true;
            RecordScatteredDependencyAssetsSelected = false;
            DeterministicAssetBundleSelected = ChunkBasedCompressionSelected = true;
            UncompressedAssetBundleSelected = DisableWriteTypeTreeSelected = ForceRebuildAssetBundleSelected = IgnoreTypeTreeChangesSelected = AppendHashToAssetBundleNameSelected = false;
            OutputDirectory = string.Empty;
        }

        public string ProductName
        {
            get
            {
                return PlayerSettings.productName;
            }
        }

        public string CompanyName
        {
            get
            {
                return PlayerSettings.companyName;
            }
        }

        public string GameIdentifier
        {
            get
            {
#if UNITY_5_6_OR_NEWER
                return PlayerSettings.applicationIdentifier;
#else
                return PlayerSettings.bundleIdentifier;
#endif
            }
        }

        public string ApplicableGameVersion
        {
            get
            {
                return Application.version;
            }
        }

        public int InternalResourceVersion
        {
            get;
            set;
        }

        public string UnityVersion
        {
            get
            {
                return Application.unityVersion;
            }
        }

        public bool WindowsSelected
        {
            get;
            set;
        }

        public bool MacOSXSelected
        {
            get;
            set;
        }

        public bool IOSSelected
        {
            get;
            set;
        }

        public bool AndroidSelected
        {
            get;
            set;
        }

        public bool WindowsStoreSelected
        {
            get;
            set;
        }

        public bool ZipSelected
        {
            get;
            set;
        }

        public bool RecordScatteredDependencyAssetsSelected
        {
            get;
            set;
        }

        public bool UncompressedAssetBundleSelected
        {
            get;
            set;
        }

        public bool DisableWriteTypeTreeSelected
        {
            get;
            set;
        }

        public bool DeterministicAssetBundleSelected
        {
            get;
            set;
        }

        public bool ForceRebuildAssetBundleSelected
        {
            get;
            set;
        }

        public bool IgnoreTypeTreeChangesSelected
        {
            get;
            set;
        }

        public bool AppendHashToAssetBundleNameSelected
        {
            get;
            set;
        }

        public bool ChunkBasedCompressionSelected
        {
            get;
            set;
        }

        public string OutputDirectory
        {
            get;
            set;
        }

        public bool IsValidOutputDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(OutputDirectory))
                {
                    return false;
                }

                if (!Directory.Exists(OutputDirectory))
                {
                    return false;
                }

                return true;
            }
        }

        public string WorkingPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Working/", OutputDirectory);
            }
        }

        public string OutputPackagePath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Package/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string OutputFullPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Full/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string OutputPackedPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/Packed/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public string BuildReportPath
        {
            get
            {
                if (!IsValidOutputDirectory)
                {
                    return string.Empty;
                }

                return string.Format("{0}/BuildReport/{1}_{2}/", OutputDirectory, ApplicableGameVersion.Replace('.', '_'), InternalResourceVersion.ToString());
            }
        }

        public event GameFrameworkAction<int, int> OnLoadingAssetBundle = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnLoadCompleted = null;

        public event GameFrameworkAction<int, int> OnAnalyzingAsset = null;

        public event GameFrameworkAction OnAnalyzeCompleted = null;

        public event GameFrameworkFunc<string, float, bool> ProcessingAssetBundle = null;

        public event GameFrameworkAction<BuildTarget, string, int, int, int, int> ProcessAssetBundleComplete = null;

        public event GameFrameworkAction<string> BuildAssetBundlesError = null;

        public bool Load()
        {
            string configurationName = Utility.Path.GetCombinePath(Application.dataPath, ConfigurationName);
            if (!File.Exists(configurationName))
            {
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configurationName);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("UnityGameFramework");
                XmlNode xmlEditor = xmlRoot.SelectSingleNode("AssetBundleBuilder");
                XmlNode xmlSettings = xmlEditor.SelectSingleNode("Settings");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlSettings.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    switch (xmlNode.Name)
                    {
                        case "InternalResourceVersion":
                            InternalResourceVersion = int.Parse(xmlNode.InnerText) + 1;
                            break;
                        case "WindowsSelected":
                            WindowsSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "MacOSXSelected":
                            MacOSXSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "IOSSelected":
                            IOSSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "AndroidSelected":
                            AndroidSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "WindowsStoreSelected":
                            WindowsStoreSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "ZipSelected":
                            ZipSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "RecordScatteredDependencyAssetsSelected":
                            RecordScatteredDependencyAssetsSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "UncompressedAssetBundleSelected":
                            UncompressedAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            if (UncompressedAssetBundleSelected)
                            {
                                ChunkBasedCompressionSelected = false;
                            }
                            break;
                        case "DisableWriteTypeTreeSelected":
                            DisableWriteTypeTreeSelected = bool.Parse(xmlNode.InnerText);
                            if (DisableWriteTypeTreeSelected)
                            {
                                IgnoreTypeTreeChangesSelected = false;
                            }
                            break;
                        case "DeterministicAssetBundleSelected":
                            DeterministicAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "ForceRebuildAssetBundleSelected":
                            ForceRebuildAssetBundleSelected = bool.Parse(xmlNode.InnerText);
                            break;
                        case "IgnoreTypeTreeChangesSelected":
                            IgnoreTypeTreeChangesSelected = bool.Parse(xmlNode.InnerText);
                            if (IgnoreTypeTreeChangesSelected)
                            {
                                DisableWriteTypeTreeSelected = false;
                            }
                            break;
                        case "AppendHashToAssetBundleNameSelected":
                            AppendHashToAssetBundleNameSelected = false;
                            break;
                        case "ChunkBasedCompressionSelected":
                            ChunkBasedCompressionSelected = bool.Parse(xmlNode.InnerText);
                            if (ChunkBasedCompressionSelected)
                            {
                                UncompressedAssetBundleSelected = false;
                            }
                            break;
                        case "OutputDirectory":
                            OutputDirectory = xmlNode.InnerText;
                            break;
                    }
                }
            }
            catch
            {
                File.Delete(configurationName);
                return false;
            }

            return true;
        }

        public bool Save()
        {
            string configurationName = Utility.Path.GetCombinePath(Application.dataPath, ConfigurationName);
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement xmlRoot = xmlDocument.CreateElement("UnityGameFramework");
                xmlDocument.AppendChild(xmlRoot);

                XmlElement xmlBuilder = xmlDocument.CreateElement("AssetBundleBuilder");
                xmlRoot.AppendChild(xmlBuilder);

                XmlElement xmlSettings = xmlDocument.CreateElement("Settings");
                xmlBuilder.AppendChild(xmlSettings);

                XmlElement xmlElement = null;

                xmlElement = xmlDocument.CreateElement("InternalResourceVersion");
                xmlElement.InnerText = InternalResourceVersion.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("WindowsSelected");
                xmlElement.InnerText = WindowsSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("MacOSXSelected");
                xmlElement.InnerText = MacOSXSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("IOSSelected");
                xmlElement.InnerText = IOSSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AndroidSelected");
                xmlElement.InnerText = AndroidSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("WindowsStoreSelected");
                xmlElement.InnerText = WindowsStoreSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ZipSelected");
                xmlElement.InnerText = ZipSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("RecordScatteredDependencyAssetsSelected");
                xmlElement.InnerText = RecordScatteredDependencyAssetsSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("UncompressedAssetBundleSelected");
                xmlElement.InnerText = UncompressedAssetBundleSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("DisableWriteTypeTreeSelected");
                xmlElement.InnerText = DisableWriteTypeTreeSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("DeterministicAssetBundleSelected");
                xmlElement.InnerText = DeterministicAssetBundleSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ForceRebuildAssetBundleSelected");
                xmlElement.InnerText = ForceRebuildAssetBundleSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("IgnoreTypeTreeChangesSelected");
                xmlElement.InnerText = IgnoreTypeTreeChangesSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AppendHashToAssetBundleNameSelected");
                xmlElement.InnerText = AppendHashToAssetBundleNameSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("ChunkBasedCompressionSelected");
                xmlElement.InnerText = ChunkBasedCompressionSelected.ToString();
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("OutputDirectory");
                xmlElement.InnerText = OutputDirectory.ToString();
                xmlSettings.AppendChild(xmlElement);

                xmlDocument.Save(configurationName);
                return true;
            }
            catch
            {
                File.Delete(configurationName);
                return false;
            }
        }

        public byte[] Compress(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return bytes;
            }

            MemoryStream memoryStream = null;
            try
            {
                memoryStream = new MemoryStream();
                using (GZipOutputStream gZipOutputStream = new GZipOutputStream(memoryStream))
                {
                    gZipOutputStream.Write(bytes, 0, bytes.Length);
                }

                return memoryStream.ToArray();
            }
            finally
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }

        public byte[] Decompress(byte[] bytes)
        {
            throw new GameFrameworkException("Decompress is not implemented.");
        }

        public bool BuildAssetBundles()
        {
            if (!IsValidOutputDirectory)
            {
                return false;
            }

            Utility.Zip.SetZipHelper(this);

            if (Directory.Exists(OutputPackagePath))
            {
                Directory.Delete(OutputPackagePath, true);
            }

            Directory.CreateDirectory(OutputPackagePath);

            if (Directory.Exists(OutputFullPath))
            {
                Directory.Delete(OutputFullPath, true);
            }

            Directory.CreateDirectory(OutputFullPath);

            if (Directory.Exists(OutputPackedPath))
            {
                Directory.Delete(OutputPackedPath, true);
            }

            Directory.CreateDirectory(OutputPackedPath);

            if (Directory.Exists(BuildReportPath))
            {
                Directory.Delete(BuildReportPath, true);
            }

            Directory.CreateDirectory(BuildReportPath);

            BuildAssetBundleOptions buildAssetBundleOptions = GetBuildAssetBundleOptions();
            m_BuildReport.Initialize(BuildReportPath, ProductName, CompanyName, GameIdentifier, ApplicableGameVersion, InternalResourceVersion, UnityVersion,
                WindowsSelected, MacOSXSelected, IOSSelected, AndroidSelected, WindowsStoreSelected, ZipSelected, RecordScatteredDependencyAssetsSelected, (int)buildAssetBundleOptions, m_AssetBundleDatas);

            try
            {
                m_VersionListDatas.Clear();

                m_BuildReport.LogInfo("Build Start Time: {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

                m_BuildReport.LogInfo("Start prepare AssetBundle collection...");
                if (!m_AssetBundleCollection.Load())
                {
                    m_BuildReport.LogError("Can not parse 'AssetBundleCollection.xml', please use 'AssetBundle Editor' tool first.");
                    m_BuildReport.SaveReport();
                    return false;
                }

                m_BuildReport.LogInfo("Prepare AssetBundle collection complete.");
                m_BuildReport.LogInfo("Start analyze assets dependency...");

                m_AssetBundleAnalyzerController.Analyze();

                m_BuildReport.LogInfo("Analyze assets dependency complete.");
                m_BuildReport.LogInfo("Start prepare build map...");

                AssetBundleBuild[] buildMap = GetBuildMap();
                if (buildMap == null || buildMap.Length <= 0)
                {
                    m_BuildReport.LogError("Build map is empty.");
                    m_BuildReport.SaveReport();
                    return false;
                }

                m_BuildReport.LogInfo("Prepare build map complete.");
                m_BuildReport.LogInfo("Start build AssetBundles for selected build targets...");

                if (WindowsSelected)
                {
                    BuildAssetBundles(buildMap, buildAssetBundleOptions, ZipSelected, BuildTarget.StandaloneWindows);
                }

                if (MacOSXSelected)
                {
                    BuildAssetBundles(buildMap, buildAssetBundleOptions, ZipSelected, BuildTarget.StandaloneOSXIntel);
                }

                if (IOSSelected)
                {
                    BuildAssetBundles(buildMap, buildAssetBundleOptions, ZipSelected, BuildTarget.iOS);
                }

                if (AndroidSelected)
                {
                    BuildAssetBundles(buildMap, buildAssetBundleOptions, ZipSelected, BuildTarget.Android);
                }

                if (WindowsStoreSelected)
                {
                    BuildAssetBundles(buildMap, buildAssetBundleOptions, ZipSelected, BuildTarget.WSAPlayer);
                }

                ProcessRecord(OutputDirectory);

                m_BuildReport.LogInfo("Build AssetBundles for selected build targets complete.");
                m_BuildReport.SaveReport();
                return true;
            }
            catch (Exception exception)
            {
                m_BuildReport.LogError(exception.Message);
                m_BuildReport.SaveReport();
                if (BuildAssetBundlesError != null)
                {
                    BuildAssetBundlesError(exception.Message);
                }

                return false;
            }
        }

        private void BuildAssetBundles(AssetBundleBuild[] buildMap, BuildAssetBundleOptions buildOptions, bool zip, BuildTarget buildTarget)
        {
            m_BuildReport.LogInfo("Start build AssetBundles for '{0}'...", buildTarget.ToString());

            string buildTargetUrlName = GetBuildTargetName(buildTarget);

            string workingPath = string.Format("{0}{1}/", WorkingPath, buildTargetUrlName);
            m_BuildReport.LogInfo("Working path is '{0}'.", workingPath);

            string outputPackagePath = string.Format("{0}{1}/", OutputPackagePath, buildTargetUrlName);
            Directory.CreateDirectory(outputPackagePath);
            m_BuildReport.LogInfo("Output package path is '{0}'.", outputPackagePath);

            string outputFullPath = string.Format("{0}{1}/", OutputFullPath, buildTargetUrlName);
            Directory.CreateDirectory(outputFullPath);
            m_BuildReport.LogInfo("Output full path is '{0}'.", outputFullPath);

            string outputPackedPath = string.Format("{0}{1}/", OutputPackedPath, buildTargetUrlName);
            Directory.CreateDirectory(outputPackedPath);
            m_BuildReport.LogInfo("Output packed path is '{0}'.", outputPackedPath);

            // Clean working path
            List<string> validNames = new List<string>();
            foreach (AssetBundleBuild i in buildMap)
            {
                string assetBundleName = GetAssetBundleFullName(i.assetBundleName, i.assetBundleVariant);
                validNames.Add(assetBundleName);
            }

            if (Directory.Exists(workingPath))
            {
                Uri workingUri = new Uri(workingPath, UriKind.RelativeOrAbsolute);
                string[] fileNames = Directory.GetFiles(workingPath, "*", SearchOption.AllDirectories);
                foreach (string fileName in fileNames)
                {
                    if (fileName.EndsWith(".manifest"))
                    {
                        continue;
                    }

                    string relativeName = workingUri.MakeRelativeUri(new Uri(fileName)).ToString();
                    if (!validNames.Contains(relativeName))
                    {
                        File.Delete(fileName);
                    }
                }

                string[] manifestNames = Directory.GetFiles(workingPath, "*.manifest", SearchOption.AllDirectories);
                foreach (string manifestName in manifestNames)
                {
                    if (!File.Exists(Path.GetFileNameWithoutExtension(manifestName)))
                    {
                        File.Delete(manifestName);
                    }
                }

                Utility.Path.RemoveEmptyDirectory(workingPath);
            }

            if (!Directory.Exists(workingPath))
            {
                Directory.CreateDirectory(workingPath);
            }

            // Build AssetBundles
            m_BuildReport.LogInfo("Unity start build AssetBundles for '{0}'...", buildTarget.ToString());
            AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(workingPath, buildMap, buildOptions, buildTarget);
            if (assetBundleManifest == null)
            {
                m_BuildReport.LogError("Build AssetBundles for '{0}' failure.", buildTarget.ToString());
                return;
            }

            m_BuildReport.LogInfo("Unity build AssetBundles for '{0}' complete.", buildTarget.ToString());

            // Process AssetBundles
            for (int i = 0; i < buildMap.Length; i++)
            {
                string assetBundleFullName = GetAssetBundleFullName(buildMap[i].assetBundleName, buildMap[i].assetBundleVariant);
                if (ProcessingAssetBundle != null)
                {
                    if (ProcessingAssetBundle(assetBundleFullName, (float)(i + 1) / buildMap.Length))
                    {
                        m_BuildReport.LogWarning("The build has been canceled by user.");
                        return;
                    }
                }

                m_BuildReport.LogInfo("Start process '{0}' for '{1}'...", assetBundleFullName, buildTarget.ToString());

                ProcessAssetBundle(workingPath, outputPackagePath, outputFullPath, outputPackedPath, zip, buildTarget, buildMap[i].assetBundleName, buildMap[i].assetBundleVariant);

                m_BuildReport.LogInfo("Process '{0}' for '{1}' complete.", assetBundleFullName, buildTarget.ToString());
            }

            ProcessPackageList(outputPackagePath, buildTarget);
            m_BuildReport.LogInfo("Process package list for '{0}' complete.", buildTarget.ToString());

            VersionListData versionListData = ProcessVersionList(outputFullPath, buildTarget);
            m_BuildReport.LogInfo("Process version list for '{0}' complete.", buildTarget.ToString());

            ProcessReadOnlyList(outputPackedPath, buildTarget);
            m_BuildReport.LogInfo("Process readonly list for '{0}' complete.", buildTarget.ToString());

            m_VersionListDatas.Add(buildTarget, versionListData);

            if (ProcessAssetBundleComplete != null)
            {
                ProcessAssetBundleComplete(buildTarget, versionListData.Path, versionListData.Length, versionListData.HashCode, versionListData.ZipLength, versionListData.ZipHashCode);
            }

            m_BuildReport.LogInfo("Build AssetBundles for '{0}' success.", buildTarget.ToString());
        }

        private void ProcessAssetBundle(string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, bool zip, BuildTarget buildTarget, string assetBundleName, string assetBundleVariant)
        {
            string assetBundleFullName = GetAssetBundleFullName(assetBundleName, assetBundleVariant);
            AssetBundleData assetBundleData = m_AssetBundleDatas[assetBundleFullName];
            string workingName = Utility.Path.GetCombinePath(workingPath, assetBundleFullName);

            byte[] bytes = File.ReadAllBytes(workingName);
            int length = bytes.Length;
            byte[] hashBytes = Utility.Verifier.GetCrc32(bytes);
            int hashCode = Utility.Converter.GetIntFromBytes(hashBytes);

            if (assetBundleData.LoadType == AssetBundleLoadType.LoadFromMemoryAndQuickDecrypt)
            {
                bytes = GetQuickXorBytes(bytes, hashBytes);
            }
            else if (assetBundleData.LoadType == AssetBundleLoadType.LoadFromMemoryAndDecrypt)
            {
                bytes = GetXorBytes(bytes, hashBytes);
            }

            // Package AssetBundle
            string packageName = Utility.Path.GetResourceNameWithSuffix(Utility.Path.GetCombinePath(outputPackagePath, assetBundleFullName));
            string packageDirectoryName = Path.GetDirectoryName(packageName);
            if (!Directory.Exists(packageDirectoryName))
            {
                Directory.CreateDirectory(packageDirectoryName);
            }

            File.WriteAllBytes(packageName, bytes);

            // Packed AssetBundle
            if (assetBundleData.Packed)
            {
                string packedName = Utility.Path.GetResourceNameWithSuffix(Utility.Path.GetCombinePath(outputPackedPath, assetBundleFullName));
                string packedDirectoryName = Path.GetDirectoryName(packedName);
                if (!Directory.Exists(packedDirectoryName))
                {
                    Directory.CreateDirectory(packedDirectoryName);
                }

                File.Copy(packageName, packedName);
            }

            // Compress AssetBundle
            string fullName = Utility.Path.GetResourceNameWithCrc32AndSuffix(Utility.Path.GetCombinePath(outputFullPath, assetBundleFullName), hashCode);
            string fullDirectoryName = Path.GetDirectoryName(fullName);
            if (!Directory.Exists(fullDirectoryName))
            {
                Directory.CreateDirectory(fullDirectoryName);
            }

            int zipLength = length;
            int zipHashCode = hashCode;
            if (zip)
            {
                byte[] zipBytes = Utility.Zip.Compress(bytes);
                zipLength = zipBytes.Length;
                zipHashCode = Utility.Converter.GetIntFromBytes(Utility.Verifier.GetCrc32(zipBytes));
                File.WriteAllBytes(fullName, zipBytes);
            }
            else
            {
                File.WriteAllBytes(fullName, bytes);
            }

            assetBundleData.AddCode(buildTarget, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessPackageList(string outputPackagePath, BuildTarget buildTarget)
        {
            byte[] encryptBytes = new byte[4];
            Utility.Random.GetRandomBytes(encryptBytes);

            string packageListPath = Utility.Path.GetCombinePath(outputPackagePath, VersionListFileName);
            using (FileStream fileStream = new FileStream(packageListPath, FileMode.CreateNew, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(PackageListHeader);
                    binaryWriter.Write(PackageListVersion);
                    binaryWriter.Write(encryptBytes);

                    byte[] applicableGameVersionBytes = GetXorBytes(Utility.Converter.GetBytesFromString(ApplicableGameVersion), encryptBytes);
                    binaryWriter.Write((byte)applicableGameVersionBytes.Length);
                    binaryWriter.Write(applicableGameVersionBytes);
                    binaryWriter.Write(InternalResourceVersion);

                    binaryWriter.Write(m_AssetBundleDatas.Count);
                    if (m_AssetBundleDatas.Count > ushort.MaxValue)
                    {
                        throw new GameFrameworkException("Package list can only contains 65535 resources in version 0.");
                    }

                    foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
                    {
                        byte[] nameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetBundleData.Name), encryptBytes);
                        if (nameBytes.Length > byte.MaxValue)
                        {
                            throw new GameFrameworkException(string.Format("AssetBundle name '{0}' is too long.", assetBundleData.Name));
                        }

                        binaryWriter.Write((byte)nameBytes.Length);
                        binaryWriter.Write(nameBytes);

                        if (assetBundleData.Variant == null)
                        {
                            binaryWriter.Write((byte)0);
                        }
                        else
                        {
                            byte[] variantBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetBundleData.Variant), encryptBytes);
                            if (variantBytes.Length > byte.MaxValue)
                            {
                                throw new GameFrameworkException(string.Format("AssetBundle variant '{0}' is too long.", assetBundleData.Variant));
                            }

                            binaryWriter.Write((byte)variantBytes.Length);
                            binaryWriter.Write(variantBytes);
                        }

                        binaryWriter.Write((byte)assetBundleData.LoadType);
                        AssetBundleCode assetBundleCode = assetBundleData.GetCode(buildTarget);
                        binaryWriter.Write(assetBundleCode.Length);
                        binaryWriter.Write(assetBundleCode.HashCode);

                        string[] assetNames = assetBundleData.GetAssetNames();
                        binaryWriter.Write(assetNames.Length);
                        foreach (string assetName in assetNames)
                        {
                            byte[] assetNameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetName), Utility.Converter.GetBytesFromInt(assetBundleCode.HashCode));
                            if (assetNameBytes.Length > byte.MaxValue)
                            {
                                throw new GameFrameworkException(string.Format("Asset name '{0}' is too long.", assetName));
                            }

                            binaryWriter.Write((byte)assetNameBytes.Length);
                            binaryWriter.Write(assetNameBytes);

                            AssetData assetData = assetBundleData.GetAssetData(assetName);
                            string[] dependencyAssetNames = assetData.GetDependencyAssetNames();
                            binaryWriter.Write(dependencyAssetNames.Length);
                            foreach (string dependencyAssetName in dependencyAssetNames)
                            {
                                byte[] dependencyAssetNameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(dependencyAssetName), Utility.Converter.GetBytesFromInt(assetBundleCode.HashCode));
                                if (dependencyAssetNameBytes.Length > byte.MaxValue)
                                {
                                    throw new GameFrameworkException(string.Format("Dependency asset name '{0}' is too long.", dependencyAssetName));
                                }

                                binaryWriter.Write((byte)dependencyAssetNameBytes.Length);
                                binaryWriter.Write(dependencyAssetNameBytes);
                            }
                        }
                    }

                    // TODO: Resource group.
                    binaryWriter.Write(0);

                    binaryWriter.Close();
                }
            }

            File.Move(packageListPath, Utility.Path.GetResourceNameWithSuffix(packageListPath));
        }

        private VersionListData ProcessVersionList(string outputFullPath, BuildTarget buildTarget)
        {
            byte[] encryptBytes = new byte[4];
            Utility.Random.GetRandomBytes(encryptBytes);

            string versionListPath = Utility.Path.GetCombinePath(outputFullPath, VersionListFileName);
            using (FileStream fileStream = new FileStream(versionListPath, FileMode.CreateNew, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(VersionListHeader);
                    binaryWriter.Write(VersionListVersion);
                    binaryWriter.Write(encryptBytes);

                    byte[] applicableGameVersionBytes = GetXorBytes(Utility.Converter.GetBytesFromString(ApplicableGameVersion), encryptBytes);
                    binaryWriter.Write((byte)applicableGameVersionBytes.Length);
                    binaryWriter.Write(applicableGameVersionBytes);
                    binaryWriter.Write(InternalResourceVersion);

                    binaryWriter.Write(m_AssetBundleDatas.Count);
                    if (m_AssetBundleDatas.Count > ushort.MaxValue)
                    {
                        throw new GameFrameworkException("Version list can only contains 65535 resources in version 0.");
                    }

                    foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
                    {
                        byte[] nameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetBundleData.Name), encryptBytes);
                        if (nameBytes.Length > byte.MaxValue)
                        {
                            throw new GameFrameworkException(string.Format("AssetBundle name '{0}' is too long.", assetBundleData.Name));
                        }

                        binaryWriter.Write((byte)nameBytes.Length);
                        binaryWriter.Write(nameBytes);

                        if (assetBundleData.Variant == null)
                        {
                            binaryWriter.Write((byte)0);
                        }
                        else
                        {
                            byte[] variantBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetBundleData.Variant), encryptBytes);
                            if (variantBytes.Length > byte.MaxValue)
                            {
                                throw new GameFrameworkException(string.Format("AssetBundle variant '{0}' is too long.", assetBundleData.Variant));
                            }

                            binaryWriter.Write((byte)variantBytes.Length);
                            binaryWriter.Write(variantBytes);
                        }

                        binaryWriter.Write((byte)assetBundleData.LoadType);
                        AssetBundleCode assetBundleCode = assetBundleData.GetCode(buildTarget);
                        binaryWriter.Write(assetBundleCode.Length);
                        binaryWriter.Write(assetBundleCode.HashCode);
                        binaryWriter.Write(assetBundleCode.ZipLength);
                        binaryWriter.Write(assetBundleCode.ZipHashCode);

                        string[] assetNames = assetBundleData.GetAssetNames();
                        binaryWriter.Write(assetNames.Length);
                        foreach (string assetName in assetNames)
                        {
                            byte[] assetNameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetName), Utility.Converter.GetBytesFromInt(assetBundleCode.HashCode));
                            if (assetNameBytes.Length > byte.MaxValue)
                            {
                                throw new GameFrameworkException(string.Format("Asset name '{0}' is too long.", assetName));
                            }

                            binaryWriter.Write((byte)assetNameBytes.Length);
                            binaryWriter.Write(assetNameBytes);

                            AssetData assetData = assetBundleData.GetAssetData(assetName);
                            string[] dependencyAssetNames = assetData.GetDependencyAssetNames();
                            binaryWriter.Write(dependencyAssetNames.Length);
                            foreach (string dependencyAssetName in dependencyAssetNames)
                            {
                                byte[] dependencyAssetNameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(dependencyAssetName), Utility.Converter.GetBytesFromInt(assetBundleCode.HashCode));
                                if (dependencyAssetNameBytes.Length > byte.MaxValue)
                                {
                                    throw new GameFrameworkException(string.Format("Dependency asset name '{0}' is too long.", dependencyAssetName));
                                }

                                binaryWriter.Write((byte)dependencyAssetNameBytes.Length);
                                binaryWriter.Write(dependencyAssetNameBytes);
                            }
                        }
                    }

                    // TODO: Resource group.
                    binaryWriter.Write(0);

                    binaryWriter.Close();
                }
            }

            byte[] bytes = File.ReadAllBytes(versionListPath);
            int length = bytes.Length;
            byte[] hashBytes = Utility.Verifier.GetCrc32(bytes);
            int hashCode = Utility.Converter.GetIntFromBytes(hashBytes);
            bytes = Utility.Zip.Compress(bytes);
            int zipLength = bytes.Length;
            File.WriteAllBytes(versionListPath, bytes);
            hashBytes = Utility.Verifier.GetCrc32(bytes);
            int zipHashCode = Utility.Converter.GetIntFromBytes(hashBytes);
            string versionListPathWithCrc32AndSuffix = Utility.Path.GetResourceNameWithCrc32AndSuffix(versionListPath, hashCode);
            File.Move(versionListPath, versionListPathWithCrc32AndSuffix);

            return new VersionListData(versionListPathWithCrc32AndSuffix, length, hashCode, zipLength, zipHashCode);
        }

        private void ProcessReadOnlyList(string outputPackedPath, BuildTarget buildTarget)
        {
            byte[] encryptBytes = new byte[4];
            Utility.Random.GetRandomBytes(encryptBytes);

            List<AssetBundleData> packedAssetBundleDatas = new List<AssetBundleData>();
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                if (!assetBundleData.Packed)
                {
                    continue;
                }

                packedAssetBundleDatas.Add(assetBundleData);
            }

            string readOnlyListPath = Utility.Path.GetCombinePath(outputPackedPath, ResourceListFileName);
            using (FileStream fileStream = new FileStream(readOnlyListPath, FileMode.CreateNew, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(ReadOnlyListHeader);
                    binaryWriter.Write(ReadOnlyListVersion);
                    binaryWriter.Write(encryptBytes);

                    binaryWriter.Write(packedAssetBundleDatas.Count);
                    foreach (AssetBundleData assetBundleData in packedAssetBundleDatas)
                    {
                        byte[] nameBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetBundleData.Name), encryptBytes);
                        if (nameBytes.Length > byte.MaxValue)
                        {
                            throw new GameFrameworkException(string.Format("AssetBundle name '{0}' is too long.", assetBundleData.Name));
                        }

                        binaryWriter.Write((byte)nameBytes.Length);
                        binaryWriter.Write(nameBytes);

                        if (assetBundleData.Variant == null)
                        {
                            binaryWriter.Write((byte)0);
                        }
                        else
                        {
                            byte[] variantBytes = GetXorBytes(Utility.Converter.GetBytesFromString(assetBundleData.Variant), encryptBytes);
                            if (variantBytes.Length > byte.MaxValue)
                            {
                                throw new GameFrameworkException(string.Format("AssetBundle variant '{0}' is too long.", assetBundleData.Variant));
                            }

                            binaryWriter.Write((byte)variantBytes.Length);
                            binaryWriter.Write(variantBytes);
                        }

                        binaryWriter.Write((byte)assetBundleData.LoadType);
                        AssetBundleCode assetBundleCode = assetBundleData.GetCode(buildTarget);
                        binaryWriter.Write(assetBundleCode.Length);
                        binaryWriter.Write(assetBundleCode.HashCode);
                    }

                    binaryWriter.Close();
                }
            }

            File.Move(readOnlyListPath, Utility.Path.GetResourceNameWithSuffix(readOnlyListPath));
        }

        private void ProcessRecord(string outputRecordPath)
        {
            string recordPath = Utility.Path.GetCombinePath(outputRecordPath, string.Format("{0}_{1}.xml", RecordName, ApplicableGameVersion.Replace('.', '_')));

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

            XmlAttribute xmlAttribute = null;
            XmlElement xmlRoot = xmlDocument.CreateElement("ResourceVersionInfo");
            xmlAttribute = xmlDocument.CreateAttribute("ApplicableGameVersion");
            xmlAttribute.Value = ApplicableGameVersion.ToString();
            xmlRoot.Attributes.SetNamedItem(xmlAttribute);
            xmlAttribute = xmlDocument.CreateAttribute("LatestInternalResourceVersion");
            xmlAttribute.Value = InternalResourceVersion.ToString();
            xmlRoot.Attributes.SetNamedItem(xmlAttribute);
            xmlDocument.AppendChild(xmlRoot);

            XmlElement xmlElement = null;
            foreach (KeyValuePair<BuildTarget, VersionListData> i in m_VersionListDatas)
            {
                xmlElement = xmlDocument.CreateElement(i.Key.ToString());
                xmlAttribute = xmlDocument.CreateAttribute("Length");
                xmlAttribute.Value = i.Value.Length.ToString();
                xmlElement.Attributes.SetNamedItem(xmlAttribute);
                xmlAttribute = xmlDocument.CreateAttribute("HashCode");
                xmlAttribute.Value = i.Value.HashCode.ToString();
                xmlElement.Attributes.SetNamedItem(xmlAttribute);
                xmlAttribute = xmlDocument.CreateAttribute("ZipLength");
                xmlAttribute.Value = i.Value.ZipLength.ToString();
                xmlElement.Attributes.SetNamedItem(xmlAttribute);
                xmlAttribute = xmlDocument.CreateAttribute("ZipHashCode");
                xmlAttribute.Value = i.Value.ZipHashCode.ToString();
                xmlElement.Attributes.SetNamedItem(xmlAttribute);

                xmlRoot.AppendChild(xmlElement);
            }

            xmlDocument.Save(recordPath);
        }

        private BuildAssetBundleOptions GetBuildAssetBundleOptions()
        {
            BuildAssetBundleOptions buildOptions = BuildAssetBundleOptions.None;

            if (UncompressedAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.UncompressedAssetBundle;
            }

            if (DisableWriteTypeTreeSelected)
            {
                buildOptions |= BuildAssetBundleOptions.DisableWriteTypeTree;
            }

            if (DeterministicAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.DeterministicAssetBundle;
            }

            if (ForceRebuildAssetBundleSelected)
            {
                buildOptions |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
            }

            if (IgnoreTypeTreeChangesSelected)
            {
                buildOptions |= BuildAssetBundleOptions.IgnoreTypeTreeChanges;
            }

            if (AppendHashToAssetBundleNameSelected)
            {
                buildOptions |= BuildAssetBundleOptions.AppendHashToAssetBundleName;
            }

            if (ChunkBasedCompressionSelected)
            {
                buildOptions |= BuildAssetBundleOptions.ChunkBasedCompression;
            }

            return buildOptions;
        }

        private AssetBundleBuild[] GetBuildMap()
        {
            m_AssetBundleDatas.Clear();

            AssetBundle[] assetBundles = m_AssetBundleCollection.GetAssetBundles();
            foreach (AssetBundle assetBundle in assetBundles)
            {
                m_AssetBundleDatas.Add(assetBundle.FullName.ToLower(), new AssetBundleData(assetBundle.Name.ToLower(), (assetBundle.Variant != null ? assetBundle.Variant.ToLower() : null), assetBundle.LoadType, assetBundle.Packed));
            }

            Asset[] assets = m_AssetBundleCollection.GetAssets();
            foreach (Asset asset in assets)
            {
                string assetName = asset.Name;
                if (string.IsNullOrEmpty(assetName))
                {
                    m_BuildReport.LogError("Can not find asset by guid '{0}'.", asset.Guid);
                    return null;
                }

                string assetFileFullName = Utility.Path.GetCombinePath(Application.dataPath, assetName.Substring(AssetsSubstringLength));
                if (!File.Exists(assetFileFullName))
                {
                    m_BuildReport.LogError("Can not find asset '{0}'.", assetFileFullName);
                    return null;
                }

                byte[] assetBytes = File.ReadAllBytes(assetFileFullName);
                int assetHashCode = Utility.Converter.GetIntFromBytes(Utility.Verifier.GetCrc32(assetBytes));

                List<string> dependencyAssetNames = new List<string>();
                DependencyData dependencyData = m_AssetBundleAnalyzerController.GetDependencyData(assetName);
                Asset[] dependencyAssets = dependencyData.GetDependencyAssets();
                foreach (Asset dependencyAsset in dependencyAssets)
                {
                    dependencyAssetNames.Add(dependencyAsset.Name);
                }

                if (RecordScatteredDependencyAssetsSelected)
                {
                    dependencyAssetNames.AddRange(dependencyData.GetScatteredDependencyAssetNames());
                }

                dependencyAssetNames.Sort();

                m_AssetBundleDatas[asset.AssetBundle.FullName.ToLower()].AddAssetData(asset.Guid, assetName, assetBytes.Length, assetHashCode, dependencyAssetNames.ToArray());
            }

            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                if (assetBundleData.AssetCount <= 0)
                {
                    m_BuildReport.LogError("AssetBundle '{0}' has no asset.", GetAssetBundleFullName(assetBundleData.Name, assetBundleData.Variant));
                    return null;
                }
            }

            AssetBundleBuild[] buildMap = new AssetBundleBuild[m_AssetBundleDatas.Count];
            int index = 0;
            foreach (AssetBundleData assetBundleData in m_AssetBundleDatas.Values)
            {
                buildMap[index].assetBundleName = assetBundleData.Name;
                buildMap[index].assetBundleVariant = assetBundleData.Variant;
                buildMap[index].assetNames = assetBundleData.GetAssetNames();
                index++;
            }

            return buildMap;
        }

        private string GetAssetBundleFullName(string assetBundleName, string assetBundleVariant)
        {
            return (!string.IsNullOrEmpty(assetBundleVariant) ? string.Format("{0}.{1}", assetBundleName, assetBundleVariant) : assetBundleName).ToLower();
        }

        private string GetBuildTargetName(BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    return "windows";
                case BuildTarget.StandaloneOSXIntel:
                    return "osx";
                case BuildTarget.iOS:
                    return "ios";
                case BuildTarget.Android:
                    return "android";
                case BuildTarget.WSAPlayer:
                    return "winstore";
                default:
                    return "notsupported";
            }
        }

        public byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
        {
            return GetXorBytes(bytes, code, QuickEncryptLength);
        }

        private byte[] GetXorBytes(byte[] bytes, byte[] code)
        {
            return GetXorBytes(bytes, code, 0);
        }

        private byte[] GetXorBytes(byte[] bytes, byte[] code, int length)
        {
            if (bytes == null)
            {
                return null;
            }

            int codeLength = code.Length;
            if (code == null || codeLength <= 0)
            {
                throw new GameFrameworkException("Code is invalid.");
            }

            int codeIndex = 0;
            int bytesLength = bytes.Length;
            if (length <= 0 || length > bytesLength)
            {
                length = bytesLength;
            }

            byte[] result = new byte[bytesLength];
            System.Buffer.BlockCopy(bytes, 0, result, 0, bytesLength);

            for (int i = 0; i < length; i++)
            {
                result[i] ^= code[codeIndex++];
                codeIndex = codeIndex % codeLength;
            }

            return result;
        }
    }
}
