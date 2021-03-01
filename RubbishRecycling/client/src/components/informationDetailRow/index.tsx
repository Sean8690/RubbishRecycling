/** @jsx jsx */
import { ReactNode } from 'react';
import { jsx, SxStyleProp } from 'theme-ui';
import { InfoLabel } from '../infoLabel';

interface InformationDetailRowProps {
  label: string;
  value: ReactNode;
  isRequired?: boolean;
  /** Width should be in REM . This will add the appropriate spacing between the label and the value fields.
   *@default 6*/
  labelWidth?: number;
}

const informationDetailRowCss: SxStyleProp = {
  marginBottom: '.5rem',
  fontSize: '0.75rem'
};

const labelDetailCss = (p: InformationDetailRowProps): SxStyleProp => ({
  display: 'inline-block',
  width: `${p.labelWidth || 6}rem`,
  overflowWrap: 'break-word',
  marginRight: '1rem'
});

const valueCss: SxStyleProp = { fontSize: '0.875rem' };

export function InformationDetailRow(props: InformationDetailRowProps) {
  const { label, value, isRequired } = props;

  return (
    <div sx={informationDetailRowCss}>
      <InfoLabel isRequired={isRequired} sx={labelDetailCss(props)}>
        {label}
      </InfoLabel>
      <span sx={valueCss}>{value}</span>
    </div>
  );
}
