﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using DN.ControleUniversidade.Domain.Interfaces.Repositories;
using DN.ControleUniversidade.Domain.Entities;
using DN.ControleUniversidade.Domain.Validation.CursoValidation;

namespace DN.ControleUniversidade.Domain.Tests.Validations
{

    [TestClass]
    public class CursoValidationsTests
    {
        TipoCurso tipoCurso;

        [TestInitialize]
        public void IniciarTeste()
        {
            tipoCurso = new TipoCurso("Tecnólogo", true);
        }

        [TestMethod]
        [TestCategory("Validations - Curso")]
        public void Um_Curso_Deve_Ser_Unico_No_Banco_De_Dados()
        {
            var curso = new Curso("Análise de Sistemas", true, tipoCurso);

            var stubRepo = MockRepository.GenerateStub<ICursoRepository>();
            stubRepo.Stub(x => x.ObterPorNome("Análise de Sistemas")).Return(curso);

            var cursoValidation = new CursoEstaConsistenteValidation(stubRepo);
            var result = cursoValidation.Validar(curso);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Erros.Any(x => x.Message == "Este curso já foi cadastrado na base de dados."));
        }

        [TestMethod]
        [TestCategory("Validations - Curso")]
        public void Para_Atualizar_Um_Curso_Deve_Ter_Nome_Diferente_Se_For_Ids_Diferentes()
        {
            var cursoParaAtualizar = new Curso("Análise de Sistemas", true, tipoCurso);
            var cursoJaCadastrado = new Curso("Análise de Sistemas", true, tipoCurso);

            var stubRepo = MockRepository.GenerateStub<ICursoRepository>();
            stubRepo.Stub(x => x.ObterPorNome("Análise de Sistemas")).Return(cursoJaCadastrado);

            var cursoValidation = new CursoEstaConsistenteParaAtualizarValidation(stubRepo);
            var result = cursoValidation.Validar(cursoParaAtualizar);

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Erros.Any(x => x.Message == "Este curso já foi cadastrado na base de dados."));

        }



    }
}
