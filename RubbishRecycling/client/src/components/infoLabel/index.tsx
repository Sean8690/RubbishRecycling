/** @jsx jsx */
import { jsx, SxStyleProp } from 'theme-ui';

const infoLabelCss: SxStyleProp = {
  color: 'b-300',
  fontWeight: 'semi-bold',
  fontSize: '0.75rem'
};

const requiredCss: SxStyleProp = {
  color: 'error'
};

interface InfoLabelProps {
  children: string;
  isRequired?: boolean;
  className?: string;
}

export function InfoLabel(props: InfoLabelProps) {
  const { children, isRequired, ...rest } = props;
  return (
    <label sx={infoLabelCss} {...rest}>
      {children}
      {isRequired && <span sx={requiredCss}> * </span>}
    </label>
  );
}
