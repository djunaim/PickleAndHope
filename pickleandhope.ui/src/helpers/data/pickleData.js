import axios from 'axios';
import {baseURL} from '../../apiKeys.json';

const getPickles = () => new Promise((resolve, reject) => {
  axios.get(`${baseURL}/api/pickles`)
    .then((result) => {
      //no need to do Object.keys because receiving data back in not a weird format
      const allPickles = result.data;
      resolve(allPickles);
    })
    .catch((err) => reject(err));
})

export { getPickles };