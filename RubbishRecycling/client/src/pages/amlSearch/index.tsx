/** @jsx jsx */
import { Illustration, Text } from '@infotrack/zenith-ui';
import { Fragment } from 'react';
import { jsx, SxStyleProp } from 'theme-ui';
import { Header } from '../../components';
import { ContentWrapper } from '../../components/contentWrapper';
import { DowJonesSearchForm } from './amlSearchForm';

const headerCss: SxStyleProp = { height: '10.5rem' };
const contentCss: SxStyleProp = { mt: '-2rem' };

export const DowJonesSearch = () => {
  return (
    <Fragment>
      <Header sx={headerCss}>
        <Text as="h1" variant="feature">
          Peps, Sanctions & Adverse Media
        </Text>
        <div sx={{ float: 'right' }}>
          <Illustration variant="locate" alt="Search Image" />
        </div>
      </Header>

      <ContentWrapper sx={contentCss}>
        <DowJonesSearchForm />
      </ContentWrapper>
    </Fragment>
  );
};
