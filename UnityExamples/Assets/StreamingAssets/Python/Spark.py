# The Spark Sensation, as defined in Sensation Editor
from pysensationcore import *
import sensation_helpers as sh   
import os

# Load an envelope file which we will threshold.
# The Intensity of the Sensation will be 1.0 above the threshold, and 0.0 below.
intensitySignal = []
with open(os.path.join(os.path.dirname(__file__), 'Spark_Envelope.txt')) as input_file:
    lines = input_file.readlines()
    for line in lines:
        intensitySignal.append(float(line.replace("\n", "")))

# Sample rate of exported intensitySignal
fs = 16000

# Number of Samples in click
NUM_SAMPLES = len(intensitySignal)

# Define a new Block to return 1.0
binaryThresholdIntensity = defineBlock("BinaryThresholdIntensity")
defineInputs(binaryThresholdIntensity, "t", "sampleRate", "threshold")
defineBlockInputDefaultValue(binaryThresholdIntensity.sampleRate, (fs, 0,0))
defineBlockInputDefaultValue(binaryThresholdIntensity.threshold, (0.055, 0,0))

def binaryThreshold(inputs):
    t = inputs[0][0] % 1
    sampleRate = inputs[1][0]
    threshold = inputs[2][0]

    # Avoid divide by zero
    if sampleRate == 0:
        return (0,0,0)
    
    # Time interval per sample
    intervalPerSample = (1/sampleRate)

    # Get the index of the closest time sample for the Click signal
    ix = int(t/intervalPerSample)

    if ix < NUM_SAMPLES - 1:
        signalValue = intensitySignal[ix]
        result = int(signalValue > threshold), 0, 0
        return result
    else:
        return (0,0,0)


defineOutputs(binaryThresholdIntensity, "out")
setMetaData(binaryThresholdIntensity.out, "Sensation-Producing", False)
defineBlockOutputBehaviour(binaryThresholdIntensity.out, binaryThreshold)

# A Lissajous Path Sensation which has its intensity modultaed by a thresholded envelope Signal
lissajousPathInstance = createInstance("LissajousPath", "lissajousPath")
thresholdIntensity = createInstance("BinaryThresholdIntensity", "intensity")

sparkBlock = sh.createSensationFromPath("Spark",
                                        {
                                            ("t", thresholdIntensity.t) : (0,0,0),
                                            ("sampleRate", thresholdIntensity.sampleRate) : (fs,0,0),
                                            ("size", lissajousPathInstance.sizeX) : (0.01, 0.0, 0.0),
                                            ("size", lissajousPathInstance.sizeY) : (0.01, 0.0, 0.0),
                                            ("A", lissajousPathInstance.paramA) : (3, 0.0, 0.0),
                                            ("B", lissajousPathInstance.paramB) : (2, 0.0, 0.0),
                                            ("intensityThreshold", thresholdIntensity.threshold) : (0.055, 0.0, 0.0)
                                        },
                                        output = lissajousPathInstance.out,
                                        renderMode = sh.RenderMode.Loop,
                                        intensity = thresholdIntensity.out,
                                        drawFrequency = 40)

setMetaData(sparkBlock.size, "Type", "Scalar")
setMetaData(sparkBlock.intensityThreshold, "Type", "Scalar")
setMetaData(sparkBlock.sampleRate, "Type", "Scalar")
setMetaData(sparkBlock.A, "Input-Visibility", False)
setMetaData(sparkBlock.B, "Input-Visibility", False)
setMetaData(sparkBlock.intensityThreshold, "Input-Visibility", False)
setMetaData(sparkBlock, "Allow-Transform", True)
