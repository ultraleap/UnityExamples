from pysensationcore import *
import sensation_helpers as sh

prefix = "point"
pathInstance = createInstance("PolylinePath", "PolylinePathInstance")
points = sh.createList(6)
connect(points["output"], pathInstance.points)

sh.createSensationFromPath("HandPolyline",
                        {
                            ("feature0", points["inputs"][0]) : (0.0, 0.0, 0.025 ),
                            ("feature1", points["inputs"][1]) : (0.024, 0.0, 0.008),
                            ("feature2", points["inputs"][2]) : (0.015, 0.0, -0.02 ),
                            ("feature3", points["inputs"][3]) : (-0.015,0.0, -0.02 ),
                            ("feature4", points["inputs"][4]) : (-0.024,0.0, 0.008 ),
                            ("feature5", points["inputs"][5]) : (0.0, 0.0, 0.025)
                        },
                        output = pathInstance.out,
                        drawFrequency = 70,
                        definedInVirtualSpace = True
                        )
