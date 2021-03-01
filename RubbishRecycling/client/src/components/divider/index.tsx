/** @jsx jsx */
import { jsx, SxStyleProp } from 'theme-ui';

interface DividerProps {
  className?: string;
}

const dividerCss: SxStyleProp = {
  margin: '1rem 0',
  border: 'none',
  borderBottom: '2px solid',
  borderColor: 'b-200'
};

export function Divider(props: DividerProps) {
  const { className } = props;
  return <hr sx={dividerCss} className={className} />;
}
