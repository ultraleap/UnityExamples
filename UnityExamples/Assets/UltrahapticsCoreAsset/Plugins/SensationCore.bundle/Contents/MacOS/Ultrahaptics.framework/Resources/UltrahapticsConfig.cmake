include("${CMAKE_CURRENT_LIST_DIR}/UltrahapticsTargets.cmake")

set(ULTRAHAPTICS_LIBRARIES $<TARGET_LINKER_FILE:Ultrahaptics::ultrahaptics>)
set(ULTRAHAPTICS_INCLUDE_DIR $<TARGET_PROPERTY:Ultrahaptics::ultrahaptics,INTERFACE_INCLUDE_DIRECTORIES>)
set(ULTRAHAPTICS_RUNTIME_LIBRARY $<TARGET_FILE:Ultrahaptics::ultrahaptics>)

if(WIN32 OR APPLE)
	include("${CMAKE_CURRENT_LIST_DIR}/UltrahapticsLibUSBTargets.cmake")
	set(ULTRAHAPTICS_LIBUSB_DLL $<TARGET_FILE:Ultrahaptics::libusb>)
endif()

if(EXISTS ${CMAKE_CURRENT_LIST_DIR}/UltrahapticsCSharpTargets.cmake)
	include("${CMAKE_CURRENT_LIST_DIR}/UltrahapticsCSharpTargets.cmake")
	set(ULTRAHAPTICS_CSHARP_FOUND TRUE)
	set(ULTRAHAPTICS_CSHARP_RUNTIME $<TARGET_FILE:Ultrahaptics::csharp>)
	set(ULTRAHAPTICS_CSHARP_35 $<TARGET_FILE_DIR:Ultrahaptics::csharp>/UltrahapticsCSharp.NET35.dll)
	set(ULTRAHAPTICS_CSHARP_40 $<TARGET_FILE_DIR:Ultrahaptics::csharp>/UltrahapticsCSharp.NET40.dll)
endif()

message(STATUS "Found Ultrahaptics: ${CMAKE_CURRENT_LIST_DIR}")
