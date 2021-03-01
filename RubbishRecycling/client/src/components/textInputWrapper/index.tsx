/** @jsx jsx */
import { TextInput, TextInputProps } from '@infotrack/zenith-ui';
import { jsx, SxStyleProp } from 'theme-ui';

interface TextInputWrapperProps extends Omit<TextInputProps, 'width'> {
  width?: string | string[];
  className?: string;
}

const textInputCss = (width?: string | string[]): SxStyleProp => ({
  width,
  display: 'inline'
});

export function TextInputWrapper(props: TextInputWrapperProps) {
  const { width, className, ...rest } = props;
  return (
    <span sx={textInputCss(width)} className={className}>
      <TextInput fullWidth={!!width} {...rest} />
    </span>
  );
}
