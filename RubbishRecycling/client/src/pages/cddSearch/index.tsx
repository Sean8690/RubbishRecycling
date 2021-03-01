/** @jsx jsx */
import { Text } from '@infotrack/zenith-ui';
import { Fragment } from 'react';
import { jsx, SxStyleProp } from 'theme-ui';
import { Header } from '../../components';
import { ContentWrapper } from '../../components/contentWrapper';
import { SearchForm } from './searchForm';

const headerCss: SxStyleProp = {
  height: '10.5rem'
};
const contentCss: SxStyleProp = {
  mt: '-2rem'
};
const subtitleCss: SxStyleProp = {
  mt: '.5rem'
};

export const CddSearch = () => {
  return (
    <Fragment>
      <Header sx={headerCss}>
        <Text as="h1" variant="feature">
          International Company Report
        </Text>
        <div sx={subtitleCss}>
          <Text as="p" variant="medium">
            Search for a company below and do your due diligence
          </Text>
        </div>
      </Header>

      <ContentWrapper sx={contentCss}>
        <SearchForm />
      </ContentWrapper>
    </Fragment>
  );
};
