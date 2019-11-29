#----------------------------------------------------------------
# Generated CMake target import file for configuration "Release".
#----------------------------------------------------------------

# Commands may need to know the format version.
set(CMAKE_IMPORT_FILE_VERSION 1)

# Import target "Ultrahaptics::ultrahaptics" for configuration "Release"
set_property(TARGET Ultrahaptics::ultrahaptics APPEND PROPERTY IMPORTED_CONFIGURATIONS RELEASE)
set_target_properties(Ultrahaptics::ultrahaptics PROPERTIES
  IMPORTED_LOCATION_RELEASE "${_IMPORT_PREFIX}/Library/Frameworks/Ultrahaptics.framework/Versions/2.6/Ultrahaptics"
  IMPORTED_SONAME_RELEASE "@rpath/Ultrahaptics.framework/Versions/2.6/Ultrahaptics"
  )

list(APPEND _IMPORT_CHECK_TARGETS Ultrahaptics::ultrahaptics )
list(APPEND _IMPORT_CHECK_FILES_FOR_Ultrahaptics::ultrahaptics "${_IMPORT_PREFIX}/Library/Frameworks/Ultrahaptics.framework/Versions/2.6/Ultrahaptics" )

# Commands beyond this point should not need to know the version.
set(CMAKE_IMPORT_FILE_VERSION)
