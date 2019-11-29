
#pragma once

#include <stdint.h>
#include "UltrahapticsDecoration.hpp"

namespace Ultrahaptics
{
   /** \brief Class to control logging.
    *
    * A reference to this class can be retrieved using UltrahapticsLibrary::getLoggingInterface().
    * It can be used to control the destination of the log, to set the log level and to set
    * a custom logging callback.
    */
   class UH_API_CLASS_DECORATION LoggingExternalInterface
   {
     public:
       /** Destroy the logging access object. */
       UH_API_DECORATION ~LoggingExternalInterface();

       /** Redirect all logging to the standard error stream.
        * This replaces any previous logging destination.
        *
        * @return True if the redirect action succeeded, false if it did not */
       UH_API_DECORATION bool redirectLoggingToStdErr();

       /** Redirect all logging to the standard output stream.
        * This replaces any previous logging destination.
        *
        * @return True if the redirect action succeeded, false if it did not */
       UH_API_DECORATION bool redirectLoggingToStdOut();

       /** Redirect all logging to a file.
        * This replaces any previous logging destination.
        *
        * @param filename The path to the log file, which may be non-existent
        *
        * @return True if the redirect action succeeded, false if it did not */
       UH_API_DECORATION bool redirectLoggingToFile(const char *filename);

       /** Disable all logging output entirely.
        *
        * @return True if the disable action succeeded, false if it did not */
       UH_API_DECORATION bool redirectLoggingToNull();

       /** Set the log level used by the Ultrahaptics SDK
        *
        * Level reference:
        *   - 0: Fatal error
        *   - 1: Error
        *   - 2: Warning
        *   - 3: Verbose
        *   - 4: Extremely verbose
        *
        * @param new_global_log_level The new log level.
        *
        * @return True if the action succeeded, false if it did not */
       UH_API_DECORATION bool setLogLevel(int new_global_log_level);

       /** Get the current log level used by the Ultrahaptics SDK
        *
        * Level reference:
        *   - 0: Fatal error
        *   - 1: Error
        *   - 2: Warning
        *   - 3: Verbose
        *   - 4: Extremely verbose
        *
        * @return The current log level */
       UH_API_DECORATION int getLogLevel() const;

       /** Type for log callback functions.
        * 
        * @param error_tag             A tag indicating where the error came from, e.g. "solver" or "driver".
        * @param triggered_log_level   The log level, see setLogLevel().
        * @param error_code            An error code - see Ultrahaptics::ErrorCode.
        * @param error_description     Description of the error.
        * @param user_pointer          The same pointer you passed to setCallbackForLogLevel().
        */
       typedef void (*LogCallback)(const char *error_tag, unsigned int triggered_log_level, uint32_t error_code, const char *error_description, void *user_pointer);

       /** Set a callback to be executed whenever a matching log entry is generated.
        *
        * This callback will be called on the same thread that made the log call, so actions taken by the
        * callback should be as lightweight as possible. This function will only be called if the global
        * log level is high enough - see setLogLevel().
        *
        * @param log_level The log level to use this callback for; note that it will be called ONLY for
        *                  messages at this exact log level
        *
        * @param log_error_function The user-provided callback function to call
        *
        * @param iuser_pointer A user-provided pointer to be passed to the callback function whenever it
        *                      is called; may be null
        *
        * @return True if the callback was set, false if it was not. */
       UH_API_DECORATION bool setCallbackForLogLevel(unsigned int log_level, LogCallback log_error_function, void *iuser_pointer);

     private:
       /** \cond EXCLUDE_FROM_DOCS */
       
       /** Create an object to access the logging interface. */
       LoggingExternalInterface();
       /** Copy constructor */
       LoggingExternalInterface(const LoggingExternalInterface &other);
       /** Assignment operator */
       LoggingExternalInterface &operator=(const LoggingExternalInterface &other);
       /** Unused pointer kept for binary compatibility. */
       void *unused;
       
       friend class LoggingImplementation;
       
       /** \endcond */
   };
}

