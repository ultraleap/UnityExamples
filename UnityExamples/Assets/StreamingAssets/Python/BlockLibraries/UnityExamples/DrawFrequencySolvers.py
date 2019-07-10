# DrawFrequency Solvers for standard geometry
from pysensationcore import *
import math

# To allow a line to be drawn with constant draw speed
# The output of drawFrequency can be fed into the RenderPath Block 
# for Path-based Sensations.
lineFreqSolver = defineBlock("DrawFrequencyLineSolver")
defineInputs(lineFreqSolver, "point1", "point2", "drawSpeed")
defineBlockInputDefaultValue(lineFreqSolver.point1, (0.0, 0, 0.0))
defineBlockInputDefaultValue(lineFreqSolver.point2, (0.0, 0, 0.0))
defineBlockInputDefaultValue(lineFreqSolver.drawSpeed, (7.0, 0, 0.0))
defineOutputs(lineFreqSolver, "drawFrequency")
setMetaData(lineFreqSolver.drawFrequency, "Sensation-Producing", False)

def lineDrawFrequencyCalc(inputs):
	pt1 = inputs[0]
	pt2 = inputs[1]
	drawSpeed = inputs[2][0]

	distance = sqrt((pt2[0] - pt1[0])**2 + (pt2[1] - pt1[1])**2 + (pt2[2] - pt1[2])**2)

	# Avoid divide by zero!
	if distance == 0:
		return (100,0,0)

	freq = float(drawSpeed)/float(distance)
	return (freq,0,0)

defineBlockOutputBehaviour(lineFreqSolver.drawFrequency, lineDrawFrequencyCalc)

# To allow a Circle to be drawn with constant draw speed
# The output of drawFrequency can be fed into the RenderPath Block 
# for Path-based Sensations.
circleFreqSolver = defineBlock("DrawFrequencyCircleSolver")
defineInputs(circleFreqSolver, "radius", "drawSpeed")

defineBlockInputDefaultValue(circleFreqSolver.radius, (0.0, 0, 0.0))
defineBlockInputDefaultValue(circleFreqSolver.drawSpeed, (7.0, 0, 0.0))
defineOutputs(circleFreqSolver, "drawFrequency")
setMetaData(circleFreqSolver.drawFrequency, "Sensation-Producing", False)

def circleDrawFrequencyCalc(inputs):
	radius = inputs[0][0]
	drawSpeed = inputs[1][0]
	distance = 2*math.pi*radius

	# Avoid divide by zero!
	if distance == 0:
		return (100,0,0)

	freq = float(drawSpeed)/float(distance)
	return (freq,0,0)

defineBlockOutputBehaviour(circleFreqSolver.drawFrequency, circleDrawFrequencyCalc)