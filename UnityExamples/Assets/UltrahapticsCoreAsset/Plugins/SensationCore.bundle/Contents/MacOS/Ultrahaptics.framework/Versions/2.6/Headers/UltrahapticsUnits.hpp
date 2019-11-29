
#pragma once

namespace Ultrahaptics
{
   /** \brief SI unit constants.
    * 
    * These provide a few convenience constants to convert from prefixed units and to
    * allow annotating values with their unit.
    * 
    * These are very simple and do not use the type system to enfore correct use of units.
    * Other libraries are available for this if desired, for example:
    * 
    * * [Boost's Unit library](http://www.boost.org/doc/libs/1_63_0/doc/html/boost_units.html)
    * * [UNITS](https://github.com/nholthaus/units)
    * * [PhysUnits](https://github.com/martinmoene/PhysUnits-CT)
    * */
   namespace Units
   {
      /** One millimetre in metres. */
      const double mm = 0.001;
      /** One millimetre in metres. */
      const double millimetres = 0.001;
      /** One centimetre in metres. */
      const double cm = 0.01;
      /** One centimetre in metres. */
      const double centimetres = 0.01;
      /** One metre in metres. Can be used to annotate a value as being in metres. */
      const double m = 1.;
      /** One metre in metres. Can be used to annotate a value as being in metres. */
      const double metres = 1.;
      /** One Herts in Hertz. Can be used to annotate a value as being in Hertz. */
      const double Hz = 1.;
      /** One Herts in Hertz. Can be used to annotate a value as being in Hertz. */
      const double hertz = 1.;
   }
}

