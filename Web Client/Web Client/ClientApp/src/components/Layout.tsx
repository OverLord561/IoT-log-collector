import React from 'react';
import { Col, Grid, Row } from 'react-bootstrap';
import NavMenu from './NavMenu';
import { RouteComponentProps } from 'react-router-dom';

type IProps = RouteComponentProps<{}>;

export default class Layout extends React.Component<IProps, any> {
  constructor(props: IProps) {
    super(props);
  }

  public render() {
    return <Grid fluid>
      <Row>
        <Col sm={3}>
          <NavMenu />
        </Col>
        <Col sm={9}>
          {this.props.children}
        </Col>
      </Row>
    </Grid>;
  }
}
