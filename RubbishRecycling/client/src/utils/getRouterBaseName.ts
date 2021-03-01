import { BASE_PATH, LEAP_BASE_PATH } from '../constants';

export const getRouterBaseName = () => {
  const currentPathName = window.location.pathname.toLowerCase();

  if (currentPathName.includes('/newwebsite/service/cdd')) {
    return LEAP_BASE_PATH;
  }

  return BASE_PATH;
};
