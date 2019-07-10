# The Click Sensation, as defined in Sensation Editor
from pysensationcore import *
import sensation_helpers as sh   
import os

# Load a click signal to act as intensity profile
# This is 1600 samples long 0.1s @16kHz.
intensitySignal = []
with open(os.path.join(os.path.dirname(__file__), 'Click_Intensity.txt')) as input_file:
    lines = input_file.readlines()
    for line in lines:
        intensitySignal.append(float(line.replace("\n", "")))

# Sample rate of exported intensitySignal
fs = 16000

# Number of Samples in click
NUM_SAMPLES = len(intensitySignal)

# Define a new Block to drive the intensity of a Circle.
clickIntensityBlock = defineBlock("ClickIntensity")
defineInputs(clickIntensityBlock, "t", "sampleRate")
defineBlockInputDefaultValue(clickIntensityBlock.sampleRate, (fs, 0,0))

def clickIntensity(inputs):
    t = inputs[0][0] % 1
    sampleRate = inputs[1][0]

    # Avoid divide by zero
    if sampleRate == 0:
        return (0,0,0)
    
    # Time interval per sample
    intervalPerSample = (1/sampleRate)

    # Get the index of the closest time sample for the Click signal
    ix = int(t/intervalPerSample)

    if ix < NUM_SAMPLES - 1:
        signalValue = intensitySignal[ix]
        return (signalValue, 0, 0)
    else:
        return (0,0,0)

defineOutputs(clickIntensityBlock, "out")
setMetaData(clickIntensityBlock.out, "Sensation-Producing", False)
defineBlockOutputBehaviour(clickIntensityBlock.out, clickIntensity)

# A Circle Path Sensation which has its intensity modultaed by a clickIntensity Signal
circlePathInstance = createInstance("CirclePath", "circlePath")
clickIntensityInstance = createInstance("ClickIntensity", "clickIntensity")

clickCircle = sh.createSensationFromPath("Click",
                                        {
                                            ("t", clickIntensityInstance.t) : (0,0,0),
                                            ("sampleRate", clickIntensityInstance.sampleRate) : (fs,0,0),
                                            ("radius", circlePathInstance.radius) : (0.02, 0.0, 0.0)
                                        },
                                        output = circlePathInstance.out,
                                        renderMode = sh.RenderMode.Loop,
                                        intensity = clickIntensityInstance.out,
                                        drawFrequency = 80)

setMetaData(clickCircle.radius, "Type", "Scalar")
setMetaData(clickCircle.sampleRate, "Type", "Scalar")
setMetaData(clickCircle, "Allow-Transform", True)