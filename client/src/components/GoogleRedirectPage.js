import { useEffect } from 'react';

const GoogleRedirectPage = () => {
    const navigate = useNavigate();

  useEffect(() => {
    // Function to handle the redirect callback
    const handleGoogleRedirect = async () => {
      // Extract the authorization code from the URL query parameters
      const searchParams = new URLSearchParams(window.location.search);
      const code = searchParams.get('code');

      // Make a request to your backend API, passing the code
      // You can use any HTTP library or the built-in fetch API
      const response = await googleCallback(code);

      // Handle the response from your backend API as needed
      if (response.ok) {
        console.log('Authentication successful');
        navigate('/login');
      } else {
        console.log('Authentication failed');
        navigate('/login');
      }
    };

    // Call the function to handle the redirect callback
    handleGoogleRedirect();
  }, []);

  return <div>Handling Google Redirect...</div>;
};

export default GoogleRedirectPage;