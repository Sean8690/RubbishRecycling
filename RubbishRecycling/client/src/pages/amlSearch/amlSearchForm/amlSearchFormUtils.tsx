import { useFormik } from 'formik';
import * as Yup from 'yup';
import {
  AmlPersonLookupRequest,
  IAmlPersonLookupRequest,
  ServiceIdentifier
} from '../../../api/api.generated';
import { INVALID_DATE, REQUIRED_FIELD } from '../../../constants/validationMessages';
import {
  getDateFromString,
  isValidDateInstance,
  momentUtil,
  subtractYearsFromNow
} from '../../../utils';

const getValidationSchema = () =>
  Yup.object({
    clientReference: Yup.string().required(REQUIRED_FIELD).nullable(true),
    givenName: Yup.string().required(REQUIRED_FIELD).nullable(true),
    middleName: Yup.string().nullable(true),
    familyName: Yup.string().required(REQUIRED_FIELD).nullable(true),
    yearOfBirth: Yup.string().test('dateOrYear', 'Invalid year', function (val) {
      const { dateOfBirth } = this.parent as AmlPersonLookupRequest;
      if (!val) return true;
      if (val) {
        const intValue = parseInt(val, 10);
        if (isNaN(intValue)) return false;
        if (intValue > new Date().getFullYear() || intValue < 1800) return false;
      }
      return !!val || !!dateOfBirth;
    }),
    dateOfBirth: Yup.mixed().test('validDateString', INVALID_DATE, function (
      val: string | Date
    ) {
      const { yearOfBirth } = this.parent as AmlPersonLookupRequest;
      if (yearOfBirth || !val) return true;
      if (
        momentUtil(subtractYearsFromNow(200)).isBefore(val) &&
        momentUtil(val).isAfter(momentUtil())
      )
        return false;
      if (typeof val === 'string') return !!getDateFromString(val);
      return isValidDateInstance(val);
    })
  });

export function useDowJonesSearchFormik(
  submitFunction: (values: IAmlPersonLookupRequest) => Promise<void>,
  clientReference: string | undefined
) {
  const formik = useFormik<IAmlPersonLookupRequest>({
    initialValues: {
      serviceIdentifier: ServiceIdentifier.CddPersonRiskLookup,
      yearOfBirth: '',
      dateOfBirth: '',
      givenName: '',
      middleName: '',
      familyName: '',
      clientReference: clientReference
    },
    isInitialValid: false,
    enableReinitialize: true,
    onSubmit: submitFunction,
    validationSchema: getValidationSchema()
  });
  return { formik };
}
