# Mux Blocks
# These Mux Blocks can be used to take in a number of inputs and select a specific input
from pysensationcore import *

# === Mux-N ===
def muxSelector(inputs):
    selector = int(inputs[0][0])
    if selector > len(inputs)-1:
      return (0,0,0)
    return inputs[selector + 1]

def createMuxN(n):
    muxBlock = defineBlock("Mux" + str(n))
    muxInputs = ["input" + str(n) for n in range(n)]
    defineInputs(muxBlock, "selector", *muxInputs)

    defineBlockInputDefaultValue(muxBlock.selector, (0, 0, 0))
    for muxInput in muxInputs:
        defineBlockInputDefaultValue(getattr(muxBlock, muxInput), (0, 0, 0))

    defineOutputs(muxBlock, "out")
    defineBlockOutputBehaviour(muxBlock.out, muxSelector)
    setMetaData(muxBlock.out, "Sensation-Producing", False)

# Create Some standard size Mux# Blocks...
createMuxN(2)
createMuxN(3)
createMuxN(4)
createMuxN(5)
createMuxN(6)
createMuxN(10)

# Utility to return a Mux Block instance of n-Inputs
def createMuxBlockInstance(numInputs=None, instanceName=None):
	"""
	Returns a Multiplexer Block Instance ("Mux<numInputs>"") with n-Inputs, specified by numInputs
	"""
	if not numInputs:
		print("createMuxBlock takes an integer argument to define how many inputs it uses!")
		return

	if not instanceName:
		instanceName = "Mux%d" % numInputs

	muxName = "Mux%d" % int(numInputs)
	muxBlock = defineBlock(muxName)
	defineInputs(muxBlock, "selector")
	defineBlockInputDefaultValue(muxBlock.selector, (0, 0, 0))

	for i in range(0,numInputs):
		inputName = "input%d" % i
		defineInputs(muxBlock, inputName)
		defineBlockInputDefaultValue(getattr(muxBlock, inputName), (0, 0, 0))

	def muxSelector(inputs):
	    selector = int(inputs[0][0])
	    return inputs[selector+1]

	defineOutputs(muxBlock, "out")
	defineBlockOutputBehaviour(muxBlock.out, muxSelector)
	setMetaData(muxBlock.out, "Sensation-Producing", False)
	
	muxInstance = createInstance(muxName, instanceName)
	return muxInstance
