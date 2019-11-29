# This  file is used for the Config-mode of find_package().
#
# It is based on this file: https://github.com/Kitware/CMake/blob/master/Modules/BasicConfigVersion-AnyNewerVersion.cmake.in
# but with added help messages in the case where you are using a 32-bit compiler
# with the 64-bit SDK (a common error). In future we will solve this by always installing
# both SDK versions so it will always work.
#
# The created file sets PACKAGE_VERSION_EXACT if the current version string and
# the requested version string are exactly the same and it sets
# PACKAGE_VERSION_COMPATIBLE if the current version is >= requested version.
# The variable CVF_VERSION must be set before calling configure_file().

# This is the version of the SDK that is installed.
set(PACKAGE_VERSION "2.6.4")

# Check that the requested version (PACKAGE_FIND_VERSION) is not less than
# the installed version.
if(PACKAGE_VERSION VERSION_LESS PACKAGE_FIND_VERSION)
  set(PACKAGE_VERSION_COMPATIBLE FALSE)
else()
  set(PACKAGE_VERSION_COMPATIBLE TRUE)
  if(PACKAGE_FIND_VERSION STREQUAL PACKAGE_VERSION)
    set(PACKAGE_VERSION_EXACT TRUE)
  endif()
endif()

# if the installed or the using project don't have CMAKE_SIZEOF_VOID_P set, ignore it:
if("${CMAKE_SIZEOF_VOID_P}" STREQUAL "" OR "8" STREQUAL "")
   return()
endif()

# check that the installed version has the same 32/64bit-ness as the one which is currently searching:
if(NOT CMAKE_SIZEOF_VOID_P STREQUAL "8")
   math(EXPR installedBits "8 * 8")
   set(PACKAGE_VERSION "${PACKAGE_VERSION} (${installedBits}bit)")
   set(PACKAGE_VERSION_UNSUITABLE TRUE)
   
   # Print a slightly friendlier message than "couldn't find a compatible version" which is rather confusing.
   math(EXPR runBits "${CMAKE_SIZEOF_VOID_P} * 8")
   message(STATUS "The ${installedBits}-bit Ultrahaptics SDK is installed but ignored because you are using a ${runBits}-bit compiler.")
endif()
