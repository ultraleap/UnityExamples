#----------------------------------------------------------------
# Generated CMake target import file for configuration "Release".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "Ultrahaptics::csharp" for configuration "Release"
set_property(TARGET Ultrahaptics::csharp APPEND PROPERTY IMPORTED_CONFIGURATIONS RELEASE)
set_target_properties(Ultrahaptics::csharp PROPERTIES
  IMPORTED_COMMON_LANGUAGE_RUNTIME_RELEASE ""
  IMPORTED_LOCATION_RELEASE "${_IMPORT_PREFIX}/Library/Frameworks/Ultrahaptics.framework/Versions/2.6/CSharp/libUltrahapticsCSharp.dylib"
  IMPORTED_NO_SONAME_RELEASE "TRUE"
  )

list(APPEND _IMPORT_CHECK_TARGETS Ultrahaptics::csharp )
list(APPEND _IMPORT_CHECK_FILES_FOR_Ultrahaptics::csharp "${_IMPORT_PREFIX}/Library/Frameworks/Ultrahaptics.framework/Versions/2.6/CSharp/libUltrahapticsCSharp.dylib" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
