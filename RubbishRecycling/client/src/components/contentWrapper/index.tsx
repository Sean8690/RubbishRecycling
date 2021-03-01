/** @jsx jsx */
import { ReactNode } from 'react';
import { jsx, SxStyleProp } from 'theme-ui';
interface ContentWrapperProps {
  children: ReactNode;
  className?: string;
}

const wrapperCss: SxStyleProp = {
  width: ['95%', '95%', '62rem'],
  margin: '0 auto',
  position: 'relative'
};

export function ContentWrapper(props: ContentWrapperProps) {
  const { children, className } = props;
  return (
    <div sx={wrapperCss} className={className}>
      {children}
    </div>
  );
}
