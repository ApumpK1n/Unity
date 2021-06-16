#import "Uptime.h"

extern "C" {
    time_t iosDeviceUptime() {
        return [Uptime uptime];
    }
}