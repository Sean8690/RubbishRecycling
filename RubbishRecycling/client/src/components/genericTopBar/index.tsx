/** @jsx jsx */
import { Logo } from '@infotrack/zenith-ui';
import { useEffect, useState } from 'react';
import { Flex, jsx, SxStyleProp } from 'theme-ui';

const topBarContainerId = 'itz-layout-root';
const topBarCss: SxStyleProp = {
  backgroundColor: '#007e9e',
  height: '3.375rem',
  pl: '2rem',
  justifyContent: 'flex-start',
  alignItems: 'center'
};
const linkCss: SxStyleProp = {
  ml: '1rem',
  borderLeft: '1px solid white',
  padding: '0 1rem',
  textDecoration: 'none',
  color: 'b-000'
};
export function GenericTopBar() {
  const [showGenericTopBar, setShowGenericTopBar] = useState(false);
  //check if Zenith topbar is present
  useEffect(() => {
    const setGenericTopBarVisibility = () => {
      const topBarHasBeenLoaded = !!document.getElementById(topBarContainerId);
      setShowGenericTopBar(!topBarHasBeenLoaded);
    };
    setGenericTopBarVisibility();

    //just in case TopBar takes a bit to load, recheck this after a few milliseconds
    setTimeout(setGenericTopBarVisibility, 400);
  }, []);
  if (showGenericTopBar)
    return (
      <Flex sx={topBarCss}>
        <Logo variant="light" href="/" />
        <a href="/" sx={linkCss}>
          Home
        </a>
      </Flex>
    );
  return null;
}
