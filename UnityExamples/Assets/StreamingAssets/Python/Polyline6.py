from pysensationcore import *
import sensation_helpers as sh

pathInstance = createInstance("PolylinePath", "PolylinePathInstance")
pointCount = 6
prefix = "point"
points = sh.createList(pointCount)
connect(points["output"], pathInstance.points)

polyline = sh.createSensationFromPath("Polyline6",
                                      {
                                          ("point0", points["inputs"][0]) : (0.0, 0.025, 0.0),
                                          ("point1", points["inputs"][1]) : (0.024, 0.008, 0.0),
                                          ("point2", points["inputs"][2]) : (0.015, -0.02, 0.0),
                                          ("point3", points["inputs"][3]) : (-0.015, -0.02, 0.0),
                                          ("point4", points["inputs"][4]) : (-0.024, 0.008, 0.0),
                                          ("point5", points["inputs"][5]) : (0.0, 0.025, 0.0)
                                      },
                                      output = pathInstance.out,
                                      drawFrequency = 70
                                      )

setMetaData(polyline, "Allow-Transform", True)

for inputName in [prefix + str(i) for i in range(pointCount)]:
    input = getattr(polyline, inputName)
    setMetaData(input, "Input-Group", "points")

defineBlockInputDefaultValue(polyline.point0, (0.0, 0.025, 0.0))
defineBlockInputDefaultValue(polyline.point1, (0.024, 0.008, 0.0))
defineBlockInputDefaultValue(polyline.point2, (0.015, -0.02, 0.0))
defineBlockInputDefaultValue(polyline.point3, (-0.015, -0.02, 0.0))
defineBlockInputDefaultValue(polyline.point4, (-0.024, 0.008, 0.0))
defineBlockInputDefaultValue(polyline.point5, (0.0, 0.025, 0.0))
