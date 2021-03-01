/** @jsx jsx */
import { jsx } from 'theme-ui';
import { ButtonProps, Button } from '@infotrack/zenith-ui';

/** To be used with fromik forms, to avoid double click issue */
export const SubmitButton = (props: ButtonProps) => {
  const { ...rest } = props;

  return (
    <Button
      type="submit"
      /* Form element blur causes the DOM to rerender and mouse up isn't captured by the same button anymore, which causes a "double-click" issue */
      onMouseDown={(e) => e.preventDefault()}
      {...rest}>
      {props.children}
    </Button>
  );
};
