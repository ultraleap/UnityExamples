#----------------------------------------------------------------
# Generated CMake target import file for configuration "Release".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "Ultrahaptics::libusb" for configuration "Release"
set_property(TARGET Ultrahaptics::libusb APPEND PROPERTY IMPORTED_CONFIGURATIONS RELEASE)
set_target_properties(Ultrahaptics::libusb PROPERTIES
  IMPORTED_LOCATION_RELEASE "${_IMPORT_PREFIX}/Library/Frameworks/Ultrahaptics.framework/Versions/./libusb-1.0.dylib"
  IMPORTED_SONAME_RELEASE "@rpath/libusb-1.0.dylib"
  )

list(APPEND _IMPORT_CHECK_TARGETS Ultrahaptics::libusb )
list(APPEND _IMPORT_CHECK_FILES_FOR_Ultrahaptics::libusb "${_IMPORT_PREFIX}/Library/Frameworks/Ultrahaptics.framework/Versions/./libusb-1.0.dylib" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
