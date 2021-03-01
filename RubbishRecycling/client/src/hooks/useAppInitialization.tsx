import { useCallback, useEffect, useState } from 'react';
import { ClientVersionCheck } from '@infotrack/zenith-ui';
import { ENV } from '../config/env';

const clientVersionCheck = new ClientVersionCheck({
  endpoint: '/service/cdd/version',
  currentVersion: ENV.buildNr
});

export function useAppInitialization() {
  const [shouldRender, setShouldRender] = useState(false);

  const onLoad = useCallback(() => {
    setShouldRender(true);
    clientVersionCheck.startVersionChecks();
  }, []);

  useEffect(() => {
    window.addEventListener('layoutLoaded', onLoad);
    window.dispatchEvent(new CustomEvent('consumerLoaded'));
    return () => window.removeEventListener('layoutLoaded', onLoad);
  }, [onLoad]);

  return { shouldRender };
}
