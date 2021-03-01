import { ToastContextType } from '@infotrack/zenith-ui';

declare global {
  interface Window {
    __hsToastContext: ToastContextType;
    __hsServiceInterruptionContext: {
      doInterruptionCheck: (service: string | string[]) => void;
    };
    __hsIsRunningOnLeap: boolean;
    __hsIsIntegratedUser: boolean;
    __hsBuildLinkUrl: () => any;
    google: any;
  }
}
