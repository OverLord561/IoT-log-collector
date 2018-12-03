import React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import SignUp from './components/SignUp/SignUp';

export default () => (
  <Layout>
    <Route exact path='/' component={Home} />
    <Route path='/sign-up' component={SignUp} />
    {/* <Route path='/fetchdata/:startDateIndex?' component={FetchData} /> */}
  </Layout>
);
