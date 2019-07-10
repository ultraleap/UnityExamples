# Example of a 62-point polyline, to show an Ultrahaptics Logo path
from pysensationcore import *
import sensation_helpers as sh   

# Approximate polyline trace of Ultrahaptics Logo
xyz = [(0.018600,-0.00020,0),
(0.014600,0.00360,0),
(0.005400,0.01080,0),
(0.000000,0.01400,0),
(-0.011000,0.01940,0),
(-0.021600,0.02260,0),
(-0.031400,0.02400,0),
(-0.039600,0.02440,0),
(-0.041400,0.02060,0),
(-0.035400,0.01560,0),
(-0.026600,0.00800,0),
(-0.020000,0.00160,0),
(-0.014200,-0.00520,0),
(-0.008200,-0.01340,0),
(-0.003600,-0.02120,0),
(0.000000,-0.02440,0),
(0.003600,-0.02120,0),
(0.008600,-0.01280,0),
(0.012800,-0.00700,0),
(0.018000,-0.00060,0),
(0.025000,0.00660,0),
(0.031600,0.01260,0),
(0.037600,0.01720,0),
(0.041400,0.02120,0),
(0.039800,0.02440,0),
(0.033400,0.02440,0),
(0.024800,0.02300,0),
(0.016200,0.02100,0),
(0.010200,0.01920,0),
(0.004200,0.01660,0),
(-0.005000,0.01140,0),
(-0.005400,0.01080,0),
(-0.013000,0.00480,0),
(-0.014600,0.00360,0),
(-0.018400,-0.00020,0),
(-0.014800,-0.00460,0),
(-0.011000,-0.00060,0),
(0.000000,0.00780,0),
(0.008200,0.01240,0),
(0.018200,0.01600,0),
(0.025600,0.01780,0),
(0.030400,0.01840,0),
(0.024400,0.01360,0),
(0.021000,0.01060,0),
(0.015600,0.00500,0),
(0.010200,-0.00180,0),
(0.006200,-0.00680,0),
(0.001800,-0.01300,0),
(0.000000,-0.01640,0),
(-0.005000,-0.00820,0),
(-0.010200,-0.00180,0),
(-0.016400,0.00600,0),
(-0.021400,0.01080,0),
(-0.026600,0.01540,0),
(-0.030200,0.01840,0),
(-0.022400,0.01720,0),
(-0.015400,0.01540,0),
(-0.009000,0.01280,0),
(-0.002600,0.00920,0),
(0.003000,0.00600,0),
(0.009000,0.00140,0),
(0.015000,-0.00420,0),
(0.018600,-0.00020,0)]

numPoints = len(xyz)
polylinePath = createInstance("PolylinePath", "PolylinePathInstance")
prefix = "point"
points = sh.createList(numPoints)

pointInputs = {}

for n in range(0,numPoints):
    pointInputs[("point%i" % n, points["inputs"][n])] = (xyz[n][0],xyz[n][1],xyz[n][2])

# Connect the points list to the PolylinePath
connect(points["output"], polylinePath.points)

polyline = sh.createSensationFromPath("UltrahapticsLogo",
                                         pointInputs,
                                         output = polylinePath.out,
                                         drawFrequency = 70,
                                         renderMode = sh.RenderMode.Loop
                                         )
for inputName in [prefix + str(i) for i in range(numPoints)]:
    point_input = getattr(polyline, inputName)
    setMetaData(point_input, "Input-Group", "points")
    setMetaData(point_input, "Input-Visibility", False)

setMetaData(polyline, "Allow-Transform", True)
