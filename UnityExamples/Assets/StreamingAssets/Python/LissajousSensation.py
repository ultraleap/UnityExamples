from pysensationcore import *

import sensation_helpers as sh

lissajousPath = createInstance("LissajousPath", "LissajousInstance")

lissajous = sh.createSensationFromPath("LissajousSensation",
                                       {
                                           ("sizeX", lissajousPath.sizeX) : (0.01, 0.0, 0.0),
                                           ("sizeY", lissajousPath.sizeY) : (0.01, 0.0, 0.0),
                                           ("paramA", lissajousPath.paramA) : (3.0, 0.0, 0.0),
                                           ("paramB", lissajousPath.paramB) : (2.0, 0.0, 0.0)
                                       },
                                       output = lissajousPath.out,
                                       drawFrequency = 40,
                                       renderMode=sh.RenderMode.Loop
                                       )

setMetaData(lissajous, "Allow-Transform", True)
setMetaData(lissajous.sizeX, "Type", "Scalar")
setMetaData(lissajous.sizeY, "Type", "Scalar")
setMetaData(lissajous.paramA, "Type", "Scalar")
setMetaData(lissajous.paramB, "Type", "Scalar")
