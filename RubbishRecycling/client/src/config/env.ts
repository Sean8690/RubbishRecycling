interface ClientEnv {
  buildNr: string;
}

declare global {
  interface Window {
    __HS_ENV: ClientEnv;
  }
}

// For testing purposes, set the ENV to dummy values
// 999.0.0 should never be less than the actual build number
export const ENV =
  window.__HS_ENV && window.__HS_ENV.buildNr ? window.__HS_ENV : { buildNr: '999.0.0' };
