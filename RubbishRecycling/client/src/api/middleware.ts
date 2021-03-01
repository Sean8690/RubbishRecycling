/** Customizing fetch options to be used by the generated client api. */
export const httpMiddleware = {
  fetch: async (url: string, options: RequestInit): Promise<Response> => {
    options.credentials = 'same-origin';
    let response: Response;
    try {
      response = await fetch(url, options);
      if (response.status === 401) redirectToLogin();
      // If the user is under a reseller account, API is going to return forbidden status
      // then we just redirect the user to home page
      if (response.status === 403) window.location.href = '/';
      if (!response.ok) {
        handleDefault(response);
        // Error is handled here, stop it from bubbling into Nswag client
        return new Response(undefined, {
          status: 200
        });
      }
      return response;
    } catch (error) {
      console.log(error);
      return new Response(
        'Error occurred in nswag middleware when trying to call the API',
        {
          status: 500
        }
      );
    }
  }
};

const handleDefault = (response: Response) => {
  const error = {
    title: `Error ${response.status}`,
    message: 'Something went wrong. Please try again later.'
  };

  window.__hsToastContext.error(error);
};

const redirectToLogin = () => {
  if (window.location.href.includes('://localhost')) {
    console.error({
      title: 'Error 401',
      message:
        "Not authenticated and you appear to be running on localhost therefore we can't redirect you."
    });
  } else {
    const loginPath = '/Home/Login';
    const encodedReturnUri = encodeURIComponent(
      window.location.pathname + window.location.search
    );
    window.location.href = `${loginPath}?ReturnUrl=${encodedReturnUri}`;
  }
};

export const handleErrors = (response: Response) => {
  switch (response.status) {
    case 401:
      return redirectToLogin();
    default:
      return handleDefault(response);
  }
};
