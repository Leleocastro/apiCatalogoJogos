using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var orders = await _orderService.Obter(pagina, quantidade);

            if (orders.Count() == 0)
            {
                return NoContent();
            }

            return Ok(orders);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idOrder">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja jogo com este id</response>
        [HttpGet("{idOrder:guid}")]
        public async Task<ActionResult<OrderViewModel>> Obter([FromRoute]Guid idOrder)
        {
            var order = await _orderService.Obter(idOrder);

            if(order == null)
                return NoContent();

            return Ok(order);
        }

        /// <summary>
        /// Inserir um jogo no catálogo
        /// </summary>
        /// <param name="orderInputModel">Dados do jogo a ser inserido</param>
        /// <response code="200">Cao o jogo seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma ImageUrl</response>
        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> InserirOrder([FromBody]OrderInputModel orderInputModel)
        {
            try
            {
                var order = await _orderService.Inserir(orderInputModel);

                return Ok(order.Id);
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(ex);
            }
        }

        /// <summary>
        /// Excluir um jogo
        /// </summary>
        /// /// <param name="idOrder">Id do jogo a ser excluído</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response> 
        [HttpDelete("{idOrder:guid}")]
        public async Task<ActionResult> ApagarOrder([FromRoute] Guid idOrder)
        {
            try
            {
                await _orderService.Remover(idOrder);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe esta ordem");
            }
        }
    }
}
