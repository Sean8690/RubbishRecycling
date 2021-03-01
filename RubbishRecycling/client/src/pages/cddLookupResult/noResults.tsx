/**@jsx jsx */
import { Button, Illustration, Text } from '@infotrack/zenith-ui';
import { jsx, SxStyleProp } from 'theme-ui';

interface NoResultsProps {
  onBack: () => void;
}

const mainWrapperCss: SxStyleProp = { textAlign: 'center' };
const illustrationWrapperCss: SxStyleProp = {
  mb: '1rem',
  img: { display: 'block', margin: '0 auto' }
};
const titleWrapperCss: SxStyleProp = { margin: '1rem auto .5rem auto' };
const descriptionWrapperCss: SxStyleProp = { mb: '1.125rem' };

export function NoResults(props: NoResultsProps) {
  const { onBack } = props;
  return (
    <div sx={mainWrapperCss}>
      <div sx={illustrationWrapperCss}>
        <Illustration variant="empty" />
      </div>
      <div sx={titleWrapperCss}>
        <Text fullWidth as="h1" variant="feature">
          No results found
        </Text>
      </div>
      <div sx={descriptionWrapperCss}>
        <Text fullWidth margin="1rem auto" as="p" variant="small">
          Your search returned no matches
        </Text>
      </div>
      <Button variant="link" onClick={onBack}>
        Try another search
      </Button>
    </div>
  );
}
