/** @jsx jsx */
import { Fragment, ReactNode } from 'react';
import { jsx, SxStyleProp } from 'theme-ui';
import { GenericTopBar } from '../genericTopBar';

const containerCss: SxStyleProp = {
  backgroundColor: 'b-100',
  position: 'relative',
  padding: '2rem'
};

interface HeaderProps {
  className?: string;
  children: ReactNode;
}

export const Header = (props: HeaderProps) => {
  const { children, className } = props;
  return (
    <Fragment>
      <GenericTopBar />
      <div sx={containerCss} className={className}>
        {children}
      </div>
    </Fragment>
  );
};
