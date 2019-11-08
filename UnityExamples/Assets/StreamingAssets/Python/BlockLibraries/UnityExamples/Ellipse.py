from pysensationcore import *
import sensation_helpers as sh

lissajous = createInstance("LissajousPath", "LissajousPathInstance")
connect(Constant((1,0,0)), lissajous.paramA)
connect(Constant((1,0,0)), lissajous.paramB)

ellipse = sh.createSensationFromPath("Ellipse",
                                      {
                                          ("sizeX", lissajous.sizeX) : (0.01, 0.5, 0.0),
                                          ("sizeY", lissajous.sizeY) : (0.02, 0.5, 0.0),
                                      },
                                      output = lissajous.out,
                                      renderMode = sh.RenderMode.Loop,
                                      drawFrequency = 60
                                      )

setMetaData(ellipse, "Allow-Transform", True)
setMetaData(ellipse.sizeX, "Type", "Scalar")
setMetaData(ellipse.sizeY, "Type", "Scalar")