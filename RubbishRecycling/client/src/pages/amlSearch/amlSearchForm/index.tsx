/** @jsx jsx */
import {
  ButtonGroup,
  ButtonGroupOption,
  Card,
  useSpinner,
  MappingContextProps,
  MappingContext
} from '@infotrack/zenith-ui';
import { KeyboardEvent, useState, useContext } from 'react';
import { useHistory } from 'react-router-dom';
import { Flex, jsx } from 'theme-ui';
import {
  AmlPersonLookupRequest,
  IAmlPersonLookupRequest,
  PersonSearchApi,
  ServiceIdentifier2
} from '../../../api';
import {
  DateInputWrapper,
  Divider,
  SubmitButton,
  TextInputWrapper
} from '../../../components';
import { ROUTES } from '../../../constants';
import { useDowJonesContext } from '../../../contexts';
import { useGetQuote } from '../../../hooks';
import { getTextInputProps, getDateStringForRequest, momentUtil } from '../../../utils';
import { ConfirmOrderModal } from './confirmOrderModal';
import { useDowJonesSearchFormik } from './amlSearchFormUtils';
import * as css from './searchFormCss';

type DateOrYear = 'date' | 'year';

export function DowJonesSearchForm() {
  const routeHistory = useHistory();
  const [dateOrYear, setDateOrYear] = useState<DateOrYear>('date');
  const [, setDowJonesState] = useDowJonesContext();
  const { quote } = useGetQuote(null, ServiceIdentifier2.CddPersonRiskLookup);
  const loader = useSpinner({ fullScreen: true });
  const [confimationModalOpen, setConfimationModalOpen] = useState(false);
  const mappingContext: MappingContextProps = useContext(MappingContext);
  const clientReference = mappingContext.findValueForMappingPath('ClientReference');
  const retailerReference = mappingContext.findValueForMappingPath('RetailerReference');

  const keyDownHandler = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' || e.keyCode === 13) {
      e.preventDefault();
      void formik.submitForm();
    }
  };

  const handleYearOrDateSelect = (v?: ButtonGroupOption) => {
    const selectedValue: DateOrYear = v?.value as DateOrYear;
    setDateOrYear(selectedValue);
    // if the user selected date clear year and vice versa
    const fieldToClear: keyof IAmlPersonLookupRequest & string =
      selectedValue === 'date' ? 'yearOfBirth' : 'dateOfBirth';
    void formik.setFieldValue(fieldToClear, '');
  };

  const removeEmptyOrNull = (obj: any) => {
    Object.keys(obj).forEach(
      (k) =>
        (obj[k] && typeof obj[k] === 'object' && removeEmptyOrNull(obj[k])) ||
        (!obj[k] && obj[k] !== undefined && delete obj[k])
    );
    return obj;
  };

  const onLookup = async (values: IAmlPersonLookupRequest) => {
    loader.show();
    values.dateOfBirth = values.dateOfBirth
      ? getDateStringForRequest(momentUtil(values.dateOfBirth).toDate())
      : undefined;
    values.retailerReference = retailerReference;
    const results = await PersonSearchApi.post(
      removeEmptyOrNull(new AmlPersonLookupRequest(values))
    );
    if (results.orderId) {
      setDowJonesState({
        lookupResults: results,
        searchedName: `${values.givenName} ${
          values.middleName ? values.middleName : ''
        } ${values.familyName}`
      });
      routeHistory.push(ROUTES.AML_LOOKUP_RESULT);
    }
    loader.hide();
  };
  // eslint-disable-next-line @typescript-eslint/no-misused-promises
  const { formik } = useDowJonesSearchFormik(onLookup, clientReference);

  const handleSubmitClicked = async () => {
    //set all fields as touched to show errors when triggering validation programmatically
    await formik.setTouched({
      clientReference: true,
      givenName: true,
      familyName: true,
      dateOfBirth: true,
      yearOfBirth: true
    });

    //trigger validation programmatically before open the modal
    await formik.validateForm();

    if (formik.isValid) {
      setConfimationModalOpen(true);
    }
  };

  return (
    <form>
      {loader.spinner}
      {confimationModalOpen && (
        <ConfirmOrderModal
          formik={formik}
          closeModalFunction={() => setConfimationModalOpen(false)}
        />
      )}
      <Card sx={css.cardCss} spacing="lg">
        <Flex sx={css.inputsWrapper}>
          <TextInputWrapper
            {...getTextInputProps(formik, 'clientReference')}
            labelText="Matter reference *"
            width={['100%', '45%', '13.75rem']}
            onKeyDown={keyDownHandler}
            disabled={!!clientReference}
          />
          <TextInputWrapper
            {...getTextInputProps(formik, 'givenName')}
            labelText="Given name *"
            width={['100%', '45%', '13.75rem']}
            onKeyDown={keyDownHandler}
          />
          <TextInputWrapper
            {...getTextInputProps(formik, 'middleName')}
            labelText="Middle name"
            width={['80%', '45%', '13.75rem']}
            onKeyDown={keyDownHandler}
          />
          <TextInputWrapper
            {...getTextInputProps(formik, 'familyName')}
            labelText="Family name *"
            width={['80%', '45%', '13.75rem']}
            onKeyDown={keyDownHandler}
          />
        </Flex>
        <Flex sx={{ justifyContent: 'flex-start', pt: '1rem' }}>
          <div sx={{ width: '28.5rem', pt: '1.29rem' }}>
            <ButtonGroup
              options={[
                { value: 'date', text: 'Date of birth' },
                { value: 'year', text: 'Year of birth' }
              ]}
              onSelect={handleYearOrDateSelect}
              value={dateOrYear}
            />
          </div>
          <div sx={{ ml: '1rem', width: '13.75rem' }}>
            {dateOrYear === 'date' && (
              <DateInputWrapper
                fullWidth
                formik={formik}
                name="dateOfBirth"
                labelText="Date of birth"
              />
            )}
            {dateOrYear === 'year' && (
              <TextInputWrapper
                {...getTextInputProps(formik, 'yearOfBirth')}
                labelText="Year of birth"
                width={['80%', '45%', '100%']}
                onKeyDown={keyDownHandler}
              />
            )}
          </div>
        </Flex>
      </Card>

      <div sx={css.submitSectionWrapper}>
        <Flex sx={{ justifyContent: 'flex-end' }}>
          {quote?.fee !== undefined && <span>Total Fee: ${quote?.fee.toFixed(2)}</span>}
        </Flex>
        <Divider />
        <Flex sx={css.actionButtonsWrapper}>
          <SubmitButton
            size="large"
            variant="primary"
            type="button"
            onClick={handleSubmitClicked}>
            Order
          </SubmitButton>
        </Flex>
      </div>
    </form>
  );
}
