/** @jsx jsx */
import { Button } from '@infotrack/zenith-ui';
import { Fragment } from 'react';
import { Flex, jsx } from 'theme-ui';
import { Divider } from '../../components';
import * as css from './lookupResultCss';

interface ActionSectionsProps {
  fee: number | string;
  submitHandler: () => void;
  onBackClicked: () => void;
  hasSelectedCompany: boolean;
  isOrdering: boolean;
}
export function ActionsSection(props: ActionSectionsProps) {
  const { fee, submitHandler, hasSelectedCompany, onBackClicked, isOrdering } = props;
  return (
    <Fragment>
      <Flex sx={css.feeSectionCss}>
        <span>{`${hasSelectedCompany ? '1' : 'No'} company selected`}</span>
        <span>
          <strong>Total Fee: </strong> ${fee}
        </span>
      </Flex>
      <Divider />
      <Flex sx={css.actionsSectionCss}>
        <Button
          variant="ghost"
          size="large"
          sx={css.backButtonCss}
          onClick={onBackClicked}>
          Back
        </Button>
        <Button
          disabled={!hasSelectedCompany}
          variant="primary"
          isLoading={isOrdering}
          size="large"
          onClick={submitHandler}>
          Place order
        </Button>
      </Flex>
    </Fragment>
  );
}
