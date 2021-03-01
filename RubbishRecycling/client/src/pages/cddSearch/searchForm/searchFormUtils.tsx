import { useFormik } from 'formik';
import { SearchInputs } from '../../../models';
import * as Yup from 'yup';
import { CountryDto } from '../../../api/api.generated';
import { REQUIRED_FIELD } from '../../../constants/validationMessages';

const getValidationSchema = (countries: CountryDto[]) =>
  Yup.object({
    matterReference: Yup.string().required(REQUIRED_FIELD).nullable(true),
    countryCode: Yup.string().test('validCountry', REQUIRED_FIELD, function (val) {
      return countries.map((c) => c.kyckrCountryCode).includes(val);
    }),
    companyName: Yup.string().test(
      'nameOrNumber',
      'Should fill name or number',
      function (val) {
        const { companyNumber } = this.parent as SearchInputs;
        return !!val || !!companyNumber;
      }
    ),
    companyNumber: Yup.string().test(
      'nameOrNumber',
      'Should fill name or number',
      function (val) {
        const { companyName } = this.parent as SearchInputs;
        return !!val || !!companyName;
      }
    )
  });

export function useSearchFormik(
  submitFunction: (values: SearchInputs) => void,
  countries: CountryDto[],
  initialValues: SearchInputs
) {
  const formik = useFormik<SearchInputs>({
    initialValues,
    enableReinitialize: true,
    onSubmit: submitFunction,
    validationSchema: getValidationSchema(countries)
  });
  return { formik };
}
