import { useBreakpointIndex } from '@theme-ui/match-media';

export const useScreenUtils = () => {
  const index = useBreakpointIndex();

  const isMobile = index === 0;
  return { isMobile };
};
