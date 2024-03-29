﻿using DTO;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DataAccess
{
    public class PolicyRepository
    {
        public PolicyRepository()
        {
            this.ConfigMapping();
        }

        private AutoMapper.IMapper Mapper;

        private void ConfigMapping()
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Client, ClientDTO>();
                cfg.CreateMap<List<Client>, List<ClientDTO>>();
                cfg.CreateMap<Policy, PoliciesDTO>();
                cfg.CreateMap<List<Policy>, List<PoliciesDTO>>();
            });
            this.Mapper = config.CreateMapper();
        }

        private void GetAllClients()
        {
            WebClient webClient = new WebClient();
            Stream stream = webClient.OpenRead(@"http://www.mocky.io/v2/5808862710000087232b75ac");
            StreamReader reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            this._clients = JsonConvert.DeserializeObject<Clients>(json).clients;
        }


        private void GetAllPolicies()
        {
            WebClient webClient = new WebClient();
            Stream stream = webClient.OpenRead(@"http://www.mocky.io/v2/580891a4100000e8242b75c5");
            StreamReader reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            this._policies = JsonConvert.DeserializeObject<Policies>(json).policies;
        }


        private List<Client> _clients { get; set; }
        public List<ClientDTO> Clients
        {
            get
            {
                if (this._clients == null)
                {
                    this.GetAllClients();
                }
                //this.Mapper.Map<List<ClientDTO>>(this._clients);
                return this._clients.Select(x => this.Mapper.Map<ClientDTO>(x)).ToList();
            }
        }


        private List<Policy> _policies { get; set; }
        public List<PoliciesDTO> Policies
        {
            get
            {
                if (this._policies == null)
                {
                    this.GetAllPolicies();
                }
                //this.Mapper.Map<List<PoliciesDTO>>(this._policies);
                return this._policies.Select(x => this.Mapper.Map<PoliciesDTO>(x)).ToList();
            }
        }


        public ClientDTO GetUserFromPolicy(Guid id)
        {

            var policy = this.Policies.Where(x => x.Id==id).FirstOrDefault();
            if (policy == null)
            {
                return null;
            }
            return this.Clients.Where(x => x.Id == policy.ClientId).FirstOrDefault();

        }


        public List<PoliciesDTO> GetByName(string name)
        {
            var client = this.Clients.Where(x => x.Name == name).FirstOrDefault();
            if (client == null)
            {
                return null;
            }
            

            return this.Policies.Where(x => x.ClientId == client.Id).ToList();
        }

    }
}
