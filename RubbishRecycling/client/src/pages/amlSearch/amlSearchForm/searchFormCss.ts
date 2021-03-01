import { SxStyleProp } from 'theme-ui';

export const cardCss: SxStyleProp = { backgroundColor: 'b-000', width: '100%' };
export const andOrLabelCss: SxStyleProp = {
  mt: '2.2rem',
  color: 'b-400',
  alignSelf: 'baseline'
};

export const inputsWrapper: SxStyleProp = {
  justifyContent: 'space-between',
  flexWrap: ['wrap', 'wrap', null]
};
export const submitSectionWrapper: SxStyleProp = { mt: '3rem' };
export const actionButtonsWrapper: SxStyleProp = { justifyContent: 'flex-end' };
export const selectWrapper: SxStyleProp = { width: ['100%', '45%', '13.25rem'] };

export const inputsSectionWrapper: SxStyleProp = {
  display: 'flex',
  justifyContent: 'space-between',
  width: ['100%', '100%', '27.5rem'],
  flexWrap: ['wrap', 'wrap', null]
};

export const rightInputSectionWrapper: SxStyleProp = {
  ...inputsSectionWrapper,
  mt: ['1rem', '1rem', 0]
};
