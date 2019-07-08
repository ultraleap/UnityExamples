from pysensationcore import *
import sensation_helpers as sh

pathInstance = createInstance("PolylinePath", "PolylinePathInstance")
points = sh.createList(5)
connect(points["output"], pathInstance.points)

polyline = sh.createSensationFromPath("Square",
                                      {
                                          ("point0", points["inputs"][0]) : (-0.5, 0.5, 0.0),
                                          ("point1", points["inputs"][1]) : (0.5, 0.5, 0.0),
                                          ("point2", points["inputs"][2]) : (0.5, -0.5, 0.0),
                                          ("point3", points["inputs"][3]) : (-0.5, -0.5, 0.0),
                                          ("point4", points["inputs"][4]) : (-0.5, 0.5, 0.0)
                                      },
                                      output = pathInstance.out,
                                      renderMode = sh.RenderMode.Loop,
                                      drawFrequency = 70
                                      )

setMetaData(polyline, "Allow-Transform", True)