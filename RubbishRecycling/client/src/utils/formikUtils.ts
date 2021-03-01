import { InputVariants, SelectOption } from '@infotrack/zenith-ui';
import { FormikProps } from 'formik';

/** Returns an object with the properties used in `<TextInput>`:
 *  - `name`
 *  - `value`
 *  - `onChange`
 *  - `disabled`
 *  - `onBlur`
 *  - `variant`
 *
 *  Use it when creating a formik form field to avoid repeating yourself `{...getTextInputProps(formikProps, fieldName)}`
 * @param formik the formik props object
 * @param fieldName name of the field whose properties we are going to get.
 */
export function getTextInputProps<T>(formik: FormikProps<T>, fieldName: keyof T) {
  const hasError = formik.touched[fieldName] && formik.errors[fieldName];
  const obj: { variant?: InputVariants; helpText?: string; disabled?: boolean } = {
    variant: hasError ? 'error' : undefined,
    helpText: hasError ? (formik.errors[fieldName] as string) : '',
    disabled: formik.isSubmitting
  };

  const fieldProps = formik.getFieldProps(fieldName);
  if (fieldProps.value === null) fieldProps.value = '';

  return Object.assign(obj, fieldProps);
}

interface DateInputFromikProps {
  id: string;
  variant?: InputVariants;
  helpText?: string;
  disabled?: boolean;
  selectedDate?: Date;
  value: string | Date;
}

/** Returns an object with the formik handlers for a `DateInput` component
 *  - `id`
 *  - `variant`
 *  - `helpText`
 *  - `disabled`
 *  - `value`
 * @param formik the formik props object
 * @param fieldName name of the field whose properties we are going to get.
 */
export function getDateInputProps<T>(
  formik: FormikProps<T>,
  fieldName: keyof T
): DateInputFromikProps {
  const hasError = formik.touched[fieldName] && formik.errors[fieldName];
  const formikProps = formik.getFieldProps(fieldName);
  return {
    id: fieldName as string,
    variant: hasError ? 'error' : undefined,
    helpText: hasError ? (formik.errors[fieldName] as string) : '',
    disabled: formik.isSubmitting,
    // getting the value in the formik way here
    // eslint-disable-next-line @typescript-eslint/no-unsafe-assignment
    value: formikProps.value
  };
}

interface SelectFormikProps {
  inputVariant?: InputVariants;
  inputHelpText?: string;
  disabled?: boolean;
  onSelect?: (selectedValue?: SelectOption, e?: React.SyntheticEvent) => any;
  options: SelectOption[];
  value: string | number;
}

/** Returns the following properties for `<Select mode="input" />` component
 *  - `inputVariant`
 *  - `inputHelpText`
 *  - `disabled`
 *  - `onSelect`
 *  - `options`
 *  - `value`
 *
 *  When creating a formik form, use it to avoid repeating yourself `{...getSelectInputProps<T>(formikProps, fieldName)}`
 * @param formik the formik props object
 * @param fieldName name of the field whose properties we are going to get.
 */
export function getSelectInputProps<T>(
  formik: FormikProps<T>,
  fieldName: keyof T,
  possibleValues: SelectOption[],
  disabledCondition?: boolean
): SelectFormikProps {
  const hasError = formik.touched[fieldName] && formik.errors[fieldName];
  const formikProps = formik.getFieldProps(fieldName);
  const properties: SelectFormikProps = {
    inputVariant: hasError ? 'error' : undefined,
    inputHelpText: hasError ? (formik.errors[fieldName] as string) : '',
    disabled: formik.isSubmitting || disabledCondition,
    onSelect: (d) => {
      formik.setFieldValue(fieldName as string, d?.value as number);
    },
    options: possibleValues,
    value: formikProps.value as number
  };
  return properties;
}

/**Populates the form with the validation error messages | submits forms */
export const submitFormWithErrorFocus = (formik: FormikProps<any>) => async () => {
  await formik.submitForm();
  const errors = await formik.validateForm();
  const keys = Object.keys(errors);

  if (keys.length > 0) {
    const errorElement = document.querySelector(`input[name="${keys[0]}"]`);
    if (errorElement) {
      const element = document.getElementsByName(keys[0]);
      if (element[0].getAttribute('type') === 'checkbox')
        element[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
      else element[0].focus();
    }
  }
};
