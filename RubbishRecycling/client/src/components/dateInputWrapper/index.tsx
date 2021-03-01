/** @jsx jsx */
import { DateInput, DateInputProps } from '@infotrack/zenith-ui';
import { FormikProps } from 'formik';
import { ChangeEvent, useEffect, useState } from 'react';
import { jsx } from 'theme-ui';
import {
  getDateInputProps,
  isValidDateInstance,
  momentUtil,
  removeTimeZone,
  toAusDateFormat
} from '../../utils';

export interface DateInputWrapperProps<T>
  extends Omit<Omit<DateInputProps, 'onChange'>, 'value'> {
  /** The formik object that will handle the form */
  formik: FormikProps<T>;
  /** The property name for this field */
  name: keyof T & string;
}

/** ### To be used with formik Date types in validation schemas
 * A wrapper controlled DateInput that will trigger the onChange event with a Date object (that can be invalid when the string is not a date),
 * but will still render the string (possible incomplete) as value of the inner `input` element */
export function DateInputWrapper<T>(props: DateInputWrapperProps<T>) {
  const { name, formik, ...rest } = props;
  const formikProps = getDateInputProps(formik, name);
  const value = formikProps.value;
  const [controlledValue, setControlledValue] = useState(value ?? '');
  const [inputValue, setInputValue] = useState<string | undefined>();
  const initialSelectedDate = isValidDateInstance(momentUtil(value).toDate())
    ? (value as Date)
    : undefined;
  const [selectedDate, setSelectedDate] = useState<Date | undefined>(initialSelectedDate);

  useEffect(() => {
    const textValue = typeof value === 'string' ? value : toAusDateFormat(value);
    // count only the digits for the date, it should be DDMMYYYY for a valid date
    const inputHasValidDate =
      inputValue && inputValue.replace(/\s+|[-/]/g, '').length === 'DDMMYYYY'.length;

    // to set the initial value for inputValue when the input is prepopulated
    if (!inputValue && !!textValue) setInputValue(textValue);

    if (typeof value === 'string') setControlledValue(textValue ?? '');
    else {
      if (inputHasValidDate && isValidDateInstance(value)) {
        setSelectedDate(value);
        setControlledValue(toAusDateFormat(momentUtil(value)) ?? '');
      } else setSelectedDate(undefined);
    }
  }, [value, inputValue]);

  const handleChange = (
    event?: ChangeEvent<HTMLInputElement>,
    _stringValue?: string | undefined,
    dateValue?: Date | undefined
  ) => {
    setControlledValue(event?.target.value as string);
    setInputValue(event?.target.value as string);
    formik.setFieldValue(name, removeTimeZone(dateValue));
  };

  return (
    <DateInput
      id={formikProps.id}
      onChange={handleChange}
      selectedDate={selectedDate}
      value={controlledValue as string}
      variant={formikProps.variant}
      disabled={formikProps.disabled}
      helpText={formikProps.helpText}
      {...rest}
    />
  );
}
