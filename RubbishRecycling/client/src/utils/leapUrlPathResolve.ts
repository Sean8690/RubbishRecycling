export const getUrlPath = () => {
  return window.location.href.includes('newwebsite') ? '/newwebsite' : '';
};
