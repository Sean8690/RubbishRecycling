/** @jsx jsx */
import {
  Button,
  Card,
  Select,
  SelectOption,
  Text,
  MappingContext,
  MappingContextProps
} from '@infotrack/zenith-ui';
import { KeyboardEvent, useEffect, useState, useContext } from 'react';
import { generatePath, useHistory } from 'react-router-dom';
import { Flex, jsx } from 'theme-ui';
import { CountryDto } from '../../../api/api.generated';
import { CountryApi } from '../../../api/clientApi';
import { Divider, TextInputWrapper } from '../../../components';
import { SubmitButton } from '../../../components/submitButton';
import { EMPTY_PARAM_VALUE, ROUTES } from '../../../constants';
import { useSearchParameters } from '../../../hooks';
import { SearchInputs } from '../../../models';
import { getSelectInputProps, getTextInputProps } from '../../../utils';
import * as css from './searchFormCss';
import { useSearchFormik } from './searchFormUtils';

export function SearchForm() {
  const routeHistory = useHistory();
  const [countries, setCountries] = useState<CountryDto[]>([]);

  useEffect(() => {
    const getCountries = async () => {
      const apiResponse = (await CountryApi.getSupportedCountries()) ?? [];
      setCountries(apiResponse);
    };
    void getCountries();
  }, []);

  const onLookup = (values: SearchInputs) => {
    const route = generatePath(ROUTES.CDD_LOOKUP_RESULT, {
      countryCode: encodeURIComponent(values.countryCode),
      matterReference: encodeURIComponent(values.matterReference),
      companyName: values.companyName
        ? encodeURIComponent(values.companyName)
        : EMPTY_PARAM_VALUE,
      companyNumber: values.companyNumber
        ? encodeURIComponent(values.companyNumber)
        : EMPTY_PARAM_VALUE
    });
    routeHistory.push(route);
  };
  // if user is coming back form lookup screen, use the values in the url to populate the form
  const initialValues = useSearchParameters();
  const { formik } = useSearchFormik(onLookup, countries, initialValues);
  const mappingContext: MappingContextProps = useContext(MappingContext);
  const clientReference = mappingContext.findValueForMappingPath('ClientReference');

  const selectOptions: SelectOption[] = countries.map((c) => ({
    value: c.kyckrCountryCode ?? '',
    text: `${c.countryName ?? ''} ${c.regionName ? ` - ${c.regionName}` : ''}`
  }));

  const keyDownHandler = (e: KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' || e.keyCode === 13) {
      e.preventDefault();
      void formik.submitForm();
    }
  };

  return (
    <form>
      <Card sx={css.cardCss} spacing="lg">
        <Flex sx={css.inputsWrapper}>
          <div sx={css.inputsSectionWrapper}>
            <TextInputWrapper
              {...getTextInputProps(formik, 'matterReference')}
              labelText="Matter reference"
              width={['100%', '45%', '13.25rem']}
              onKeyDown={keyDownHandler}
              disabled={!!clientReference}
            />
            <span sx={css.selectWrapper}>
              <Select
                searchPlaceholder="Search..."
                searchable
                fullWidth
                mode="input"
                {...getSelectInputProps(formik, 'countryCode', selectOptions)}
                inputLabel="Company jurisdiction"
                maxHeight="15rem"
              />
            </span>
          </div>
          <div sx={css.rightInputSectionWrapper}>
            <TextInputWrapper
              {...getTextInputProps(formik, 'companyName')}
              labelText="Company name"
              width={['100%', '45%', '11.875rem']}
              onKeyDown={keyDownHandler}
            />
            <span sx={css.andOrLabelCss}>
              <Text as="label" variant="subtitle">
                {' '}
                and/or{' '}
              </Text>
            </span>
            <TextInputWrapper
              {...getTextInputProps(formik, 'companyNumber')}
              labelText="Company number"
              width={['80%', '45%', '11.875rem']}
              onKeyDown={keyDownHandler}
            />
          </div>
        </Flex>
      </Card>

      <div sx={css.submitSectionWrapper}>
        <Text variant="medium" as="p" margin="0 .5rem 0 0 !important">
          Click here to request a tailored search.
        </Text>
        <Button variant="link" size="large" linkUrl="/gcs/home">
          Go to KYCIT
        </Button>
        <Divider />
        <Flex sx={css.actionButtonsWrapper}>
          <SubmitButton
            size="large"
            variant="primary"
            type="button"
            onClick={formik.submitForm}>
            Lookup
          </SubmitButton>
        </Flex>
      </div>
    </form>
  );
}
