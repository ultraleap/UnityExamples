using UnityEngine;
using UnityEditor;
using System.IO;

namespace UltrahapticsCoreAsset.Examples.PolylineHandPathDesigner
{
    // Serialize out the Sensation values to a Python file
    public class SaveSensationToPythonBlock : MonoBehaviour
    {

        public SensationSource sensation;
        public UpdatePolyline6TextLabels features;
        public Polyline6DataSource polyline6DataSource;
        public TextAsset blockTextTemplate;
        public string blockName;
        public bool trackPalmOption = true;
        public bool loopOption = false;

        // Writes a Sensation Block (<blockNam>.py) file to StreamingAssets/Python
        public void WriteSensationBlockToStreamingAssets()
        {
            if (blockName.Length <= 0)
            {
                Debug.LogError("Sensation Block Name must contain at least one character!");
            }

            var blockFile = blockName + ".py";
            string path = Application.streamingAssetsPath+ "/Python/" + blockFile;
            string blockFileText = blockTextTemplate.text;

            blockFileText = blockFileText.Replace("<BLOCK_NAME>", blockName);

            for (int i = 0; i <= 5; i++) 
            {
                Vector3 pointValue = sensation.Inputs["feature" + i].Value;
                string vec3String = string.Format("({0},{1},{2})", pointValue.x, pointValue.y, pointValue.z);

                blockFileText = blockFileText.Replace("<POINT" + i + ">", vec3String);
                blockFileText = blockFileText.Replace("<FEATURE" + i + ">", polyline6DataSource.gameObjectFeatureList[i].name);
            }

            var drawFreq = sensation.Inputs["drawFrequency"].Value;
            blockFileText = blockFileText.Replace("<DRAW_FREQUENCY>", drawFreq.x.ToString());

            var trackPalmBoolOption = trackPalmOption ? "True" : "False";
            blockFileText = blockFileText.Replace("<TRACK_PALM>", trackPalmBoolOption);

            var renderModeOption = loopOption ? "sh.RenderMode.Loop" : "sh.RenderMode.Bounce";
            blockFileText = blockFileText.Replace("<RENDER_MODE>", renderModeOption);


            //Write text to the Python file
            StreamWriter writer = new StreamWriter(path, false);

            writer.Write(blockFileText);
            writer.Close();

            // Refresh the AssetData base so that the file appears and can be accessed from the BlockDropdown
            #if UNITY_EDITOR
                AssetDatabase.Refresh();
            #endif

        }
    }
}