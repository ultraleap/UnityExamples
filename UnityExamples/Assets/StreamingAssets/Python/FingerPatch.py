from pysensationcore import *
import sensation_helpers as sh

polylinePath = createInstance("PolylinePath", "FingerPatchPolylinePathInstance")
pointCount = 6
points = sh.createList(pointCount)
connect(points["output"], polylinePath.points)

fingerPatch = sh.createSensationFromPath("FingerPatch",
                                         {
                                             ("indexFinger_intermediate_position", points["inputs"][0]) : (0.0, 0.0, 0.0),
                                             ("middleFinger_intermediate_position", points["inputs"][1]) : (0.0, 0.0, 0.0),
                                             ("ringFinger_intermediate_position", points["inputs"][2]) : (0.0, 0.0, 0.0),
                                             ("ringFinger_metacarpal_position", points["inputs"][3]) : (0.0, 0.0, 0.0),
                                             ("indexFinger_metacarpal_position", points["inputs"][4]) : (0.0, 0.0, 0.0),
                                             ("indexFinger_intermediate_position", points["inputs"][5]) : (0.0, 0.0, 0.0)
                                         },
                                         output = polylinePath.out,
                                         drawFrequency = 70,
                                         definedInVirtualSpace = True,
                                         renderMode = sh.RenderMode.Loop
                                         )

for input in [fingerPatch.indexFinger_intermediate_position,
              fingerPatch.middleFinger_intermediate_position,
              fingerPatch.ringFinger_intermediate_position,
              fingerPatch.ringFinger_metacarpal_position,
              fingerPatch.indexFinger_metacarpal_position]:
    setMetaData(input, "Input-Visibility", False)
    setMetaData(input, "Input-Group", "points")
